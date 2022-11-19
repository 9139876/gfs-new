using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using GFS.BkgWorker.Enum;
using GFS.BkgWorker.Queue;
using GFS.BkgWorker.Task;

namespace GFS.BkgWorker.Worker;

public interface IWorker
{
    void EnqueueTask(BackgroundTask task, TaskPriorityEnum taskPriorityEnum);
    void CancelTask(BackgroundTask task);
    BackgroundTask[] GetTasks();
    void Start(byte threadsCount);
}

public class Worker : IWorker
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;
    private readonly TasksQueue _queue = new();
    private bool _isStarted;

    public Worker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _logger = serviceProvider.GetRequiredService<ILogger>();
    }

    public void Start(byte threadsCount)
    {
        lock (this)
        {
            if (_isStarted)
                return;

            _isStarted = true;
        }

        for (var i = 0; i < threadsCount; i++)
        {
            var thread = new Thread(DoWork) { IsBackground = true };
            thread.Start();
        }
    }

    public void EnqueueTask(BackgroundTask task, TaskPriorityEnum taskPriorityEnum)
    {
        if (!_isStarted)
            throw new InvalidOperationException("Worker is not started");

        _queue.EnqueueTask(task, taskPriorityEnum);
    }

    public void CancelTask(BackgroundTask task)
    {
        if (!_isStarted)
            throw new InvalidOperationException("Worker is not started");

        _queue.CancelTask(task);
    }

    public BackgroundTask[] GetTasks()
        => _isStarted ? _queue.GetTasks() : throw new InvalidOperationException("Worker is not started");

    private void DoWork()
    {
        using var loggerScope = _logger.BeginScope("{TaskExecutorType}", GetType().Name);

        while (true)
        {
            while (_queue.TryDequeue(out var task))
            {
                if (task == null)
                    continue;

                try
                {
                    _logger.LogDebug("Start iteration of task {Task}", task);
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var needNextIteration = task.Context.DoWork(scope.ServiceProvider).Result;

                        if (needNextIteration)
                            _queue.ReportOfSuccessIteration(task);
                        else
                            task.ReportOfComplete();
                    }

                    _logger.LogDebug("Iteration of task {Task} completed", task);
                }
                catch (Exception e)
                {
                    _logger.LogError("{Task}, {Message}, {StackTrace}", task.Serialize(), e.Message, e.StackTrace);
                    _queue.ReportOfFailIteration(task, e.Message);
                }
            }

            Thread.Sleep(1000);
        }
    }
}