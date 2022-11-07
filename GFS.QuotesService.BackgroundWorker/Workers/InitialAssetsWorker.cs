using GFS.BkgWorker.Abstraction;
using GFS.QuotesService.BackgroundWorker.TaskContexts;
using GFS.QuotesService.BL.Services;

namespace GFS.QuotesService.BackgroundWorker.Workers;

public class InitialAssetsWorker : TaskExecutor<EmptyTaskContext>
{
    private readonly IGetDataFromProviderService _getDataFromProviderService;
    
    public InitialAssetsWorker(
        ILogger logger,
        IGetDataFromProviderService getDataFromProviderService) : base(1, 1, logger, null)
    {
        _getDataFromProviderService = getDataFromProviderService;
        TryEnqueue(new EmptyTaskContext());
    }

    protected override async Task DoWorkImpl(EmptyTaskContext task)
    {
        await _getDataFromProviderService.InitialFromMainAdapter(false);
        NeedStop();
    }
}