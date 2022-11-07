namespace GFS.BkgWorker.Abstraction;

public interface ITaskGetter<TContext>
    where TContext : TaskContext
{
    ITaskGetterModel Model { get; }
    
    Task<IEnumerable<TContext>> GetTasks();
}