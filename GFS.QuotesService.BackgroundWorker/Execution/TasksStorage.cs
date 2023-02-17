using GFS.BkgWorker.Enum;
using GFS.QuotesService.Api.Common.Enum;
using GFS.QuotesService.BackgroundWorker.Api.Models;

namespace GFS.QuotesService.BackgroundWorker.Execution;

public static class TasksStorage
{
    private static readonly Dictionary<TaskStateEnum, List<BkgWorkerTask>> Tasks = new();

    static TasksStorage()
    {
        foreach (TaskStateEnum state in Enum.GetValues(typeof(TaskStateEnum)))
            Tasks.Add(state, new List<BkgWorkerTask>());
    }

    public static bool TryGetTaskForExecute(QuotesProviderTypeEnum quotesProviderType, out BkgWorkerTask? task)
    {
        lock (Tasks)
        {
            task = Tasks[TaskStateEnum.PendingExecution]
                .Where(t => t.QuotesProviderType == quotesProviderType)
                .OrderBy(t => t.GetPriority())
                .FirstOrDefault();

            if (task == null)
                return false;

            task.State = TaskStateEnum.Executing;
            Tasks[TaskStateEnum.PendingExecution].Remove(task);
            Tasks[TaskStateEnum.Executing].Add(task);

            return true;
        }
    }

    public static void AddTasks(IEnumerable<BkgWorkerTask> tasks)
    {
        lock (Tasks)
        {
            var addedTasks = tasks.ToList();
            addedTasks.ForEach(t => t.State = TaskStateEnum.PendingExecution);
            Tasks[TaskStateEnum.PendingExecution].AddRange(addedTasks);
        }
    }

    public static void CancelPendingExecutionTasks()
    {
        lock (Tasks)
        {
            Tasks[TaskStateEnum.PendingExecution].ForEach(task => task.State = TaskStateEnum.Canceled);
            Tasks[TaskStateEnum.Canceled].AddRange(Tasks[TaskStateEnum.PendingExecution]);
            Tasks[TaskStateEnum.PendingExecution].Clear();
        }
    }

    public static void ReportOfSuccess(Guid taskId)
    {
        lock (Tasks)
        {
            var task = Tasks[TaskStateEnum.Executing].Single(t => t.TaskId == taskId);

            task.State = TaskStateEnum.Completed;

            Tasks[TaskStateEnum.Executing].Remove(task);
            Tasks[TaskStateEnum.Completed].Add(task);
        }
    }

    public static void ReportOfFail(Guid taskId, string errorMessage)
    {
        lock (Tasks)
        {
            var task = Tasks[TaskStateEnum.Executing].Single(t => t.TaskId == taskId);

            task.State = TaskStateEnum.Failed;
            task.Error = errorMessage;

            Tasks[TaskStateEnum.Executing].Remove(task);
            Tasks[TaskStateEnum.Failed].Add(task);
        }
    }

    public static List<BkgWorkerTask> GetTasks(params TaskStateEnum[] states)
    {
        lock (Tasks)
        {
            return states.Any()
                ? Tasks.Keys.Where(states.Contains).SelectMany(key => Tasks[key]).ToList()
                : Tasks.Values.SelectMany(list => list).ToList();
        }
    }
}