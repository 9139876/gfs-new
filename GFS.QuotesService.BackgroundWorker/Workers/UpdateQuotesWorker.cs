using AutoMapper;
using GFS.BackgroundWorker.Workers;
using GFS.Common.Attributes;
using GFS.Common.Helpers;
using GFS.GrailCommon.Enums;
using GFS.QuotesService.Api.Enum;
using GFS.QuotesService.BL.Models;
using GFS.QuotesService.BL.Services;
using GFS.QuotesService.DAL;
using GFS.QuotesService.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GFS.QuotesService.BackgroundWorker.Workers;

internal class UpdateQuotesWorker : SimpleWorker<UpdateQuotesTaskData>
{
    private readonly IMapper _mapper;
    private readonly IOptions<WorkersSettings> _workersSettings;

    public UpdateQuotesWorker(IServiceProvider serviceProvider) : base(serviceProvider, serviceProvider.GetRequiredService<ILogger<UpdateQuotesWorker>>())
    {
        _mapper = serviceProvider.GetRequiredService<IMapper>();
        _workersSettings = serviceProvider.GetRequiredService<IOptions<WorkersSettings>>();
    }

    protected override TimeSpan SleepTime => TimeSpan.FromSeconds(_workersSettings.Value.UpdateQuotesSleepInSeconds);

    protected override async Task<List<UpdateQuotesTaskData>> GetTasksData(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<QuotesServiceDbContext>();
        
        return _mapper.Map<List<UpdateQuotesTaskData>>(await dbContext
            .GetRepository<UpdateQuotesTaskEntity>()
            .Get(task => task.IsActive)
            .Include(task => task.AssetByProvider)
            .AsNoTracking()
            .ToListAsync());
    }

    protected override async Task<TaskExecutingResult<UpdateQuotesTaskData>> DoTaskInternal(IServiceProvider serviceProvider, UpdateQuotesTaskData taskDataItem)
    {
        var dbContext = serviceProvider.GetRequiredService<QuotesServiceDbContext>();
        var quotesProviderService = serviceProvider.GetRequiredService<IQuotesProviderService>();
        
        var getQuotesBatchRequest = new GetQuotesBatchRequestModel
        {
            QuotesProviderType = taskDataItem.QuotesProviderType,
            Asset = await dbContext.GetRepository<AssetEntity>().SingleOrFailByIdAsync(taskDataItem.AssetId),
            TimeFrame = taskDataItem.TimeFrame,
            LastQuoteDate = taskDataItem.LastQuoteDate
        };

        var quotesBatchResponse = await quotesProviderService.GetQuotesBatch(getQuotesBatchRequest);

        var quotes = quotesBatchResponse.Quotes
            .Where(qe => qe.Date > taskDataItem.LastQuoteDate)
            .Distinct(new QuoteEntityComparerByTfAndDate())
            .OrderBy(qe => qe.Date)
            .ToList();

        if (quotes.Any())
        {
            using var transaction = SystemTransaction.Default();

            await dbContext.GetRepository<QuoteEntity>().BulkInsertRangeAsync(quotes);
            await dbContext.BulkSaveChangesAsync();

            var taskEntity = await dbContext.GetRepository<UpdateQuotesTaskEntity>().SingleOrFailByIdAsync(taskDataItem.EntityId);
            taskEntity.LastQuoteDate = quotes.Last().Date;
            await dbContext.SaveChangesAsync();

            transaction.Complete();
        }

        var newTask = quotesBatchResponse.IsLastBatch || !quotes.Any()
            ? null
            : taskDataItem.GetNewTask(quotesBatchResponse.NextBatchBeginningDate!.Value);

        var message = quotes.Any()
            ? $"Получено {quotes.Count} котировок, последняя от {quotes.Last().Date}"
            : "Новых котировок не получено";

        return new TaskExecutingResult<UpdateQuotesTaskData>(message, newTask);
    }
}

internal class UpdateQuotesTaskData : ILoggingSerializable
{
    public Guid EntityId { get; init; }

    public QuotesProviderTypeEnum QuotesProviderType { get; init; }

    public Guid AssetId { get; init; }

    public TimeFrameEnum TimeFrame { get; init; }

    public DateTime LastQuoteDate { get; init; }

    public string Serialize()
        => $"Получение котировок для AssetId={AssetId} провайдер '{QuotesProviderType}' таймфрейм '{Description.GetDescription(TimeFrame)}' позднее {LastQuoteDate}";

    internal UpdateQuotesTaskData GetNewTask(DateTime lastQuoteDate)
    {
        return new UpdateQuotesTaskData
        {
            EntityId = this.EntityId,
            QuotesProviderType = this.QuotesProviderType,
            AssetId = this.AssetId,
            TimeFrame = this.TimeFrame,
            LastQuoteDate = lastQuoteDate
        };
    }
}