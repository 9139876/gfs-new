using AutoMapper;
using GFS.BackgroundWorker.Workers;
using GFS.Common.Attributes;
using GFS.Common.Helpers;
using GFS.EF.Repository;
using GFS.GrailCommon.Enums;
using GFS.QuotesService.BL.Models;
using GFS.QuotesService.BL.Services;
using GFS.QuotesService.Common.Enum;
using GFS.QuotesService.DAL;
using GFS.QuotesService.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GFS.QuotesService.BackgroundWorker.Workers;

internal class UpdateQuotes : SimpleWorker<UpdateQuotesTaskData>
{
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IQuotesProviderService _quotesProviderService;

    public UpdateQuotes(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _dbContext = serviceProvider.GetRequiredService<QuotesServiceDbContext>();
        _mapper = serviceProvider.GetRequiredService<IMapper>();
        _quotesProviderService = serviceProvider.GetRequiredService<IQuotesProviderService>();
    }

    protected override async Task<List<UpdateQuotesTaskData>> GetTasksData(IServiceProvider serviceProvider)
    {
        return _mapper.Map<List<UpdateQuotesTaskData>>(await _dbContext
            .GetRepository<UpdateQuotesTaskEntity>()
            .Get(task => task.IsActive)
            .Include(task => task.AssetByProvider)
            .AsNoTracking()
            .ToListAsync());
    }

    protected override async Task<TaskExecutingResult<UpdateQuotesTaskData>> DoTaskAndReturnLoggingText(IServiceProvider serviceProvider, UpdateQuotesTaskData taskDataItem)
    {
        var quotesBatchResponse = await _quotesProviderService.GetQuotesBatch(_mapper.Map<GetQuotesBatchRequestModel2>(taskDataItem));

        var quotes = quotesBatchResponse.Quotes
            .Where(qe => qe.Date > taskDataItem.LastQuoteDate)
            .Distinct(new QuoteEntityComparerByTfAndDate())
            .OrderBy(qe => qe.Date)
            .ToList();

        if (quotes.Any())
        {
            using var transaction = SystemTransaction.Default();

            await _dbContext.GetRepository<QuoteEntity>().BulkInsertRangeAsync(quotes);
            await _dbContext.BulkSaveChangesAsync();

            var taskEntity = await _dbContext.GetRepository<UpdateQuotesTaskEntity>().SingleOrFailByIdAsync(taskDataItem.EntityId);
            taskEntity.LastQuoteDate = quotes.Last().Date;
            await _dbContext.SaveChangesAsync();
        }

        var newTask = quotesBatchResponse.IsLastBatch || !quotes.Any()
            ? null
            : taskDataItem.GetNewTask(quotes.Last().Date);

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
        => $"Получение котировок для AssetId={AssetId} провайдер {QuotesProviderType} таймфрейм {Description.GetDescription(TimeFrame)} позже {LastQuoteDate}";

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