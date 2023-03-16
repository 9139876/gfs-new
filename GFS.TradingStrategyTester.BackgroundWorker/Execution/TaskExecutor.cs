using GFS.BackgroundWorker.Execution;
using GFS.BackgroundWorker.Models;
using GFS.TradingStrategyTester.Api.Models;

namespace GFS.TradingStrategyTester.BackgroundWorker.Execution;

public class TaskExecutor : AbstractTaskExecutor<TradingStrategyTestingItemContext>
{
    private readonly ILogger _logger;

    public TaskExecutor(
        ITasksStorage<TradingStrategyTestingItemContext> tasksStorage,
        ILogger logger)
        : base(tasksStorage, logger)
    {
        _logger = logger;
    }

    protected override Predicate<TradingStrategyTestingItemContext> TasksSelector
        => _ => true;

    protected override void ExecuteInternal(TradingStrategyTestingItemContext ctx)
    {
        throw new NotImplementedException();
    }

    protected override IDisposable LoggerScope
        => _logger.BeginScope("{ExecutorNameSpace}", GetType().Namespace);

    protected override Action<Exception, BkgWorkerTask<TradingStrategyTestingItemContext>> LogErrorMessage
        => (e, task) => _logger.LogError(e, "Task {TaskType} id {TaskId} failed", nameof(TradingStrategyTestingItemContext), task.TaskId);
}