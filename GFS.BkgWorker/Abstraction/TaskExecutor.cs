using Microsoft.Extensions.Logging;

namespace GFS.BkgWorker.Abstraction;

public interface ITaskExecutor
{
    bool TryEnqueue(TaskContext taskContext, PriorityType priorityType, byte attempts);
    void NeedStop();
}

public abstract class TaskExecutor<TContext> : ITaskExecutor
    where TContext : TaskContext
{
    private readonly ITaskGetter<TContext>? _taskGetter;
    private readonly PriorityUniqueTaskQueue<TContext> _queue;
    private bool _needStop;
    private bool _isWorking = true;
    protected ILogger Logger { get; }

    protected TaskExecutor(
        byte threadsCount,
        byte queueMaxSize,
        ILogger logger,
        ITaskGetter<TContext>? taskGetter = null)
    {
        Logger = logger;
        _taskGetter = taskGetter;
        _queue = new PriorityUniqueTaskQueue<TContext>(queueMaxSize);

        for (var i = 0; i < threadsCount; i++)
        {
            var thread = new Thread(DoWork) { IsBackground = true };
            thread.Start();
        }
    }

    public bool TryEnqueue(TaskContext taskContext, PriorityType priorityType = PriorityType.Medium, byte attempts = 3)
        => taskContext is TContext context && _queue.TryEnqueue(context, priorityType, attempts);

    public void NeedStop()
        => _needStop = true;

    public bool IsWorking()
        => _isWorking;

    private void DoWork()
    {
        using var loggerScope = Logger.BeginScope("{TaskExecutorType}", GetType().Name);

        while (!_needStop)
        {
            _taskGetter?.GetTasks().Result.ToList().ForEach(task => _queue.TryEnqueue(task));

            while (_queue.TryDequeue(out var task))
            {
                if (task == null)
                    continue;

                try
                {
                    DoWorkImpl(task).Wait();
                    _queue.ReportOfSuccessTaskExecution(task);
                    Logger.LogDebug("{Task}", task.Serialize());
                }
                catch (Exception e)
                {
                    Logger.LogError("{Message}, {StackTrace}", e.Message, e.StackTrace);
                    _queue.ReportOfFailTaskExecution(task);
                }
            }

            Thread.Sleep(1000);
        }

        _isWorking = false;
    }

    protected abstract Task DoWorkImpl(TContext task);
}