using GFS.BkgWorker.Enum;
using GFS.BkgWorker.Task;

namespace GFS.BkgWorker.Queue;

public class TasksQueue
{
    private readonly List<BackgroundTask> _tasks = new();
    private uint _currentIteration = 1;

    public void EnqueueTask(BackgroundTask task, TaskPriorityEnum taskPriority)
    {
        lock (_tasks)
        {
            var existingTask = _tasks.FirstOrDefault(t => t.Serialize() == task.Serialize());

            if (existingTask != null)
            {
                existingTask.SetInQueueState();
                existingTask.SetAttempts(task.AttemptsDefault);
                existingTask.Priority = taskPriority;
                existingTask.IterationNumber = _currentIteration;
            }
            else
            {
                task.SetInQueueState();
                task.IterationNumber = _currentIteration;
                task.Priority = taskPriority;
                _tasks.Add(task);
            }
        }
    }

    public bool TryDequeue(out BackgroundTask? task)
    {
        lock (_tasks)
        {
            var activeTasks = _tasks.Where(MayBeExecution).OrderBy(t => t.Priority).ToList();

            if (!activeTasks.Any())
            {
                task = null;
                return false;
            }

            task = activeTasks.FirstOrDefault(t => t.IterationNumber <= _currentIteration);

            if (task == null)
            {
                _currentIteration++;
                task = activeTasks.First();
            }

            task.SetExecutingState();
            
            return true;
        }
    }

    public BackgroundTask[] GetTasks()
    {
        lock (_tasks)
        {
            return _tasks.ToArray();
        }
    }

    public void CancelTask(BackgroundTask task)
    {
        lock (_tasks)
        {
            var canceledTask = _tasks.SingleOrDefault(t => t.Serialize() == task.Serialize());
            canceledTask?.SetCancelState();
        }
    }

    public void ReportOfFailIteration(BackgroundTask task, string error)
    {
        task.IterationNumber++;
        task.ReportOfFailIteration(error);
    }

    public void ReportOfSuccessIteration(BackgroundTask task)
    {
        task.IterationNumber++;
        task.ReportOfSuccessIteration();
    }

    private static bool MayBeExecution(BackgroundTask taskContainer)
        => taskContainer.State is TaskStateEnum.PendingExecution; // or TaskStateEnum.ReQueuedAfterError;
}