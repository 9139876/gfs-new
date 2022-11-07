using GFS.BkgWorker.Abstraction;
using GFS.QuotesService.BackgroundWorker.TaskContexts;

namespace GFS.QuotesService.BackgroundWorker.Workers;

public class BcsExpressGetQuotesWorker : TaskExecutor<GetQuotesTaskContext>
{
    public BcsExpressGetQuotesWorker(byte threadsCount, byte queueMaxSize, ILogger logger, ITaskGetter<GetQuotesTaskContext>? taskGetter = null) : base(threadsCount, queueMaxSize,
        logger, taskGetter)
    {
    }

    protected override Task DoWorkImpl(GetQuotesTaskContext task)
    {
        throw new NotImplementedException();
    }
}