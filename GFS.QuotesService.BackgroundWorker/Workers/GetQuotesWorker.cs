using GFS.BkgWorker.Abstraction;
using GFS.QuotesService.BackgroundWorker.TaskContexts;
using GFS.QuotesService.BL.Services;

namespace GFS.QuotesService.BackgroundWorker.Workers;

public class GetQuotesWorker : TaskExecutor<GetQuotesTaskContext>
{
    private readonly IQuotesProviderService _quotesProviderService;

    public GetQuotesWorker(
        byte threadsCount,
        byte queueMaxSize,
        ILogger logger,
        IQuotesProviderService quotesProviderService,
        ITaskGetter<GetQuotesTaskContext>? taskGetter = null) : base(threadsCount, queueMaxSize, logger, taskGetter)
    {
        _quotesProviderService = quotesProviderService;
    }

    protected override async Task DoWorkImpl(GetQuotesTaskContext task)
    {
        await _quotesProviderService.GetAndSaveNextQuotesBatch(task.QuotesProviderType, task.AssetId, task.TimeFrame);
    }
}