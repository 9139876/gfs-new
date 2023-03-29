using GFS.Api.Client.Services;
using GFS.BackgroundWorker.Execution;
using GFS.BackgroundWorker.Models;
using GFS.TradingStrategyTester.Api.Models;

namespace GFS.TradingStrategyTester.BackgroundWorker.Execution;

public class TaskExecutor : AbstractTaskExecutor<TradingStrategyTestingItemContext>
{
    private readonly ILogger _logger;
    private readonly IRemoteApiClient _remoteApiClient;

    public TaskExecutor(
        ITasksStorage<TradingStrategyTestingItemContext> tasksStorage,
        ILogger logger,
        IRemoteApiClient remoteApiClient)
        : base(tasksStorage)
    {
        _logger = logger;
        _remoteApiClient = remoteApiClient;
    }

    protected override Predicate<TradingStrategyTestingItemContext> TasksSelector
        => _ => true;

    protected override void ExecuteInternal(TradingStrategyTestingItemContext ctx)
    {
        //получить все котировки всех нужных инструментов
        
        //выбрать из них даты
        
        //foreach по датам
            //foreach по инструментам
                //анализ
                //Анализ прогнозов
            //Совершение операций
    }

    protected override IDisposable LoggerScope
        => _logger.BeginScope("{ExecutorNameSpace}", GetType().Namespace);

    protected override Action<Exception, BkgWorkerTask<TradingStrategyTestingItemContext>> LogErrorMessage
        => (e, task) => _logger.LogError(e, "Task {TaskType} id {TaskId} failed", nameof(TradingStrategyTestingItemContext), task.TaskId);
}