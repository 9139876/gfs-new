using AutoMapper;
using GFS.BackgroundWorker.Workers;
using GFS.EF.Repository;
using GFS.QuotesService.BL.Services;
using GFS.QuotesService.DAL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GFS.QuotesService.BackgroundWorker.Workers;

internal class UpdateAssetsListWorker : SimpleWorker<UpdateAssetsListTaskData>
{
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IQuotesProviderService _quotesProviderService;
    private readonly IOptions<WorkersSettings> _workersSettings;

    public UpdateAssetsListWorker(IServiceProvider serviceProvider) : base(serviceProvider, serviceProvider.GetRequiredService<ILogger<UpdateAssetsListWorker>>())
    {
        _dbContext = serviceProvider.GetRequiredService<QuotesServiceDbContext>();
        _mapper = serviceProvider.GetRequiredService<IMapper>();
        _quotesProviderService = serviceProvider.GetRequiredService<IQuotesProviderService>();
        _workersSettings = serviceProvider.GetRequiredService<IOptions<WorkersSettings>>();
    }

    protected override TimeSpan SleepTime => TimeSpan.FromSeconds(_workersSettings.Value.UpdateAssetsListSleepInSeconds);

    protected override Task<List<UpdateAssetsListTaskData>> GetTasksData(IServiceProvider serviceProvider)
    {
        return Task.FromResult(new List<UpdateAssetsListTaskData> { new UpdateAssetsListTaskData() });
    }

    protected override async Task<TaskExecutingResult<UpdateAssetsListTaskData>> DoTaskInternal(IServiceProvider serviceProvider, UpdateAssetsListTaskData taskDataItem)
    {
        return new TaskExecutingResult<UpdateAssetsListTaskData>(string.Empty, null);
    }
}

internal class UpdateAssetsListTaskData : ILoggingSerializable
{
    public string Serialize()
    {
        return string.Empty;
    }
}