using GFS.BkgWorker.Abstraction;

namespace GFS.QuotesService.BackgroundWorker.TaskContexts;

public class EmptyTaskContext : TaskContext
{
    protected override string SerializeImpl()
        => string.Empty;
}