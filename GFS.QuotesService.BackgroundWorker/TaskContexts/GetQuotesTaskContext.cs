using GFS.BkgWorker.Abstraction;

namespace GFS.QuotesService.BackgroundWorker.TaskContexts;

public class GetQuotesTaskContext : TaskContext
{
    protected override string SerializeImpl()
    {
        throw new NotImplementedException();
    }
}