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

    protected override void ExecuteInternal(TradingStrategyTestingItemContext ctx, Action<string?> updateState)
    {
        //получить все котировки всех нужных инструментов
        
        //В словари, где ключ - дата
        
        //var currentDate = Дата самой первой котировки 
        
        //while (currentDate <= дата последней котировки && Portfolio говорит что еще не просрали все полимеры)
            //foreach по инструментам
                //if(quote.exist)
                    //Отложенные ордера
                    //анализ
                    //Анализ прогнозов
            //Совершение операций
            //CurrentDate++
    }

    protected override IDisposable LoggerScope
        => _logger.BeginScope("{ExecutorNameSpace}", GetType().Namespace);

    protected override Action<Exception, BkgWorkerTask<TradingStrategyTestingItemContext>> LogErrorMessage
        => (e, task) => _logger.LogError(e, "Task {TaskType} id {TaskId} failed", nameof(TradingStrategyTestingItemContext), task.TaskId);
}