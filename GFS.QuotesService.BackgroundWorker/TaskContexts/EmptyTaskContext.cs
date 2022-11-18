using GFS.BkgWorker.Abstraction;
using GFS.BkgWorker.Task;

namespace GFS.QuotesService.BackgroundWorker.TaskContexts;

public class EmptyTaskContext : TaskContext
{
    protected override string SerializeImpl()
        => string.Empty;
}