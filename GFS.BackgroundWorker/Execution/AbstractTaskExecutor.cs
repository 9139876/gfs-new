using System.Diagnostics.CodeAnalysis;
using GFS.BackgroundWorker.Models;

namespace GFS.BackgroundWorker.Execution;

public abstract class AbstractTaskExecutor<TContext>
    where TContext : class, IBkgWorkerTaskContext
{
    private readonly ITasksStorage<TContext> _tasksStorage;

    protected AbstractTaskExecutor(
        ITasksStorage<TContext> tasksStorage)
    {
        _tasksStorage = tasksStorage;
    }

    [SuppressMessage("ReSharper", "FunctionNeverReturns")]
    public void Execute()
    {
        using var loggerScope = LoggerScope;

        while (true)
        {
            while (_tasksStorage.TryGetTaskForExecute(TasksSelector, out var task))
            {
                try
                {
                    // ReSharper disable once AccessToModifiedClosure
                    ExecuteInternal(task!.Context, ( subState) => _tasksStorage.ChangeSubState(task.TaskId, subState));
                    _tasksStorage.ReportOfSuccess(task.TaskId);
                }
                catch (Exception e)
                {
                    LogErrorMessage?.Invoke(e, task!);
                    _tasksStorage.ReportOfFail(task!.TaskId, e.Message);
                }
            }

            Thread.Sleep(1000);
        }
    }

    protected abstract Predicate<TContext> TasksSelector { get; }

    protected abstract void ExecuteInternal(TContext ctx, Action<string?> updateState);

    protected abstract IDisposable LoggerScope { get; }

    protected abstract Action<Exception, BkgWorkerTask<TContext>> LogErrorMessage { get; }
}