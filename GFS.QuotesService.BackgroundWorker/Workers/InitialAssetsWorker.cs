using GFS.BkgWorker.Abstraction;
using GFS.QuotesService.BackgroundWorker.TaskContexts;
using GFS.QuotesService.BL.Services;

namespace GFS.QuotesService.BackgroundWorker.Workers;

public class InitialAssetsWorker : TaskExecutor<EmptyTaskContext>
{
    private readonly IQuotesProviderService _quotesProviderService;
    
    public InitialAssetsWorker(
        ILogger logger,
        IQuotesProviderService quotesProviderService) : base(1, 1, logger, null)
    {
        _quotesProviderService = quotesProviderService;
        TryEnqueue(new EmptyTaskContext());
    }

    protected override async Task DoWorkImpl(EmptyTaskContext task)
    {
        await _quotesProviderService.InitialAssets(false);
        NeedStop();
    }
}