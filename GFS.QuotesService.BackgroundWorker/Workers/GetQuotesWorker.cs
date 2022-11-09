using GFS.BkgWorker.Abstraction;
using GFS.QuotesService.BackgroundWorker.TaskContexts;
using GFS.QuotesService.BL.Services;

namespace GFS.QuotesService.BackgroundWorker.Workers;

public class GetQuotesWorker : TaskExecutor<GetQuotesTaskContext>
{
    private readonly IGetDataFromProviderService _getDataFromProviderService;

    public GetQuotesWorker(
        byte threadsCount,
        byte queueMaxSize,
        ILogger logger,
        IGetDataFromProviderService getDataFromProviderService,
        ITaskGetter<GetQuotesTaskContext>? taskGetter = null) : base(threadsCount, queueMaxSize, logger, taskGetter)
    {
        _getDataFromProviderService = getDataFromProviderService;
    }

    protected override async Task DoWorkImpl(GetQuotesTaskContext task)
    {
        await _getDataFromProviderService.GetAndSaveNextQuotesBatch(task.QuotesProviderType, task.AssetId, task.TimeFrame);
    }
}