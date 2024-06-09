using GFS.BackgroundWorker.Execution;
using GFS.BackgroundWorker.Models;
using GFS.TradingStrategyTester.Api.Models;
using GFS.TradingStrategyTester.BL.Services;

namespace GFS.TradingStrategyTester.BackgroundWorker.Execution;

public class TaskExecutor : AbstractTaskExecutor<TradingStrategyTestingItemContext>
{
    private readonly ILogger<TaskExecutor> _logger;
    private readonly ITestTradeOrchestratorService _testTradeOrchestratorService;

    public TaskExecutor(
        ITasksStorage<TradingStrategyTestingItemContext> tasksStorage,
        ILogger<TaskExecutor> logger,
        ITestTradeOrchestratorService testTradeOrchestratorService)
        : base(tasksStorage)
    {
        _logger = logger;
        _testTradeOrchestratorService = testTradeOrchestratorService;
    }

    protected override Predicate<TradingStrategyTestingItemContext> TasksSelector
        => _ => true;

    protected override void ExecuteInternal(TradingStrategyTestingItemContext ctx, Action<string?> updateState)
    {
        ctx.TestingItemSettings.ValidateModel();
        _testTradeOrchestratorService.Test(ctx.TestingItemSettings, updateState).Wait();
    }

    protected override IDisposable LoggerScope
        => _logger.BeginScope("{ExecutorNameSpace}", GetType().Namespace);

    protected override Action<Exception, BkgWorkerTask<TradingStrategyTestingItemContext>> LogErrorMessage
        => (e, task) => _logger.LogError(e, "Task {TaskType} id {TaskId} failed", nameof(TradingStrategyTestingItemContext), task.TaskId);
}