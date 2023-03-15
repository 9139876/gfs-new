using GFS.BackgroundWorker.Enums;
using GFS.BackgroundWorker.Models;

namespace GFS.BackgroundWorker.Execution;

public interface ITasksStorage<TContext>
    where TContext : class, IBkgWorkerTaskContext
{
    bool TryGetTaskForExecute(Predicate<TContext> predicate, out BkgWorkerTask<TContext>? task);

    void AddTasks(IEnumerable<BkgWorkerTask<TContext>> tasks);

    void CancelPendingExecutionTasks();

    void ReportOfSuccess(Guid taskId);

    void ReportOfFail(Guid taskId, string errorMessage);

    List<BkgWorkerTask<TContext>> GetTasks(params TaskStateEnum[] states);
}

public class TasksStorage<TContext> : ITasksStorage<TContext>
    where TContext : class, IBkgWorkerTaskContext
{
    private readonly Dictionary<TaskStateEnum, List<BkgWorkerTask<TContext>>> _tasks = new();

    public TasksStorage()
    {
        foreach (TaskStateEnum state in Enum.GetValues(typeof(TaskStateEnum)))
            _tasks.Add(state, new List<BkgWorkerTask<TContext>>());
    }

    public bool TryGetTaskForExecute(Predicate<TContext> predicate, out BkgWorkerTask<TContext>? task)
    {
        lock (_tasks)
        {
            task = _tasks[TaskStateEnum.PendingExecution]
                .Where(t => predicate(t.Context))
                //.Where(t => t.QuotesProviderType == quotesProviderType)
                .OrderBy(t => t.Context.GetPriority())
                .FirstOrDefault();

            if (task == null)
                return false;

            task.State = TaskStateEnum.Executing;
            _tasks[TaskStateEnum.PendingExecution].Remove(task);
            _tasks[TaskStateEnum.Executing].Add(task);

            return true;
        }
    }

    public void AddTasks(IEnumerable<BkgWorkerTask<TContext>> tasks)
    {
        lock (_tasks)
        {
            var addedTasks = tasks.ToList();
            addedTasks.ForEach(t => { t.State = TaskStateEnum.PendingExecution; });
            _tasks[TaskStateEnum.PendingExecution].AddRange(addedTasks);
        }
    }

    public void CancelPendingExecutionTasks()
    {
        lock (_tasks)
        {
            _tasks[TaskStateEnum.PendingExecution].ForEach(task =>
            {
                task.State = TaskStateEnum.Canceled;
                task.StateDate = DateTime.Now;
            });
            _tasks[TaskStateEnum.Canceled].AddRange(_tasks[TaskStateEnum.PendingExecution]);
            _tasks[TaskStateEnum.PendingExecution].Clear();
        }
    }

    public void ReportOfSuccess(Guid taskId)
    {
        lock (_tasks)
        {
            var task = _tasks[TaskStateEnum.Executing].Single(t => t.TaskId == taskId);

            task.State = TaskStateEnum.Completed;
            task.StateDate = DateTime.Now;

            _tasks[TaskStateEnum.Executing].Remove(task);
            _tasks[TaskStateEnum.Completed].Add(task);
        }
    }

    public void ReportOfFail(Guid taskId, string errorMessage)
    {
        lock (_tasks)
        {
            var task = _tasks[TaskStateEnum.Executing].Single(t => t.TaskId == taskId);

            task.State = TaskStateEnum.Failed;
            task.Error = errorMessage;
            task.StateDate = DateTime.Now;

            _tasks[TaskStateEnum.Executing].Remove(task);
            _tasks[TaskStateEnum.Failed].Add(task);
        }
    }

    public List<BkgWorkerTask<TContext>> GetTasks(params TaskStateEnum[] states)
    {
        lock (_tasks)
        {
            return states.Any()
                ? _tasks.Keys.Where(states.Contains).SelectMany(key => _tasks[key]).ToList()
                : _tasks.Values.SelectMany(list => list).ToList();
        }
    }
}