using System.Collections.Concurrent;

namespace GFS.BkgWorker;

public class TaskQueueExecutor
{
    private readonly IServiceProvider _serviceProvider;
    private readonly BlockingCollection<BkgTaskContext> _tasks = new();
    private readonly HashSet<string> _tasksIdentifiers = new();
    private byte _queueMaxSize;
    private bool _started;

    public TaskQueueExecutor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Start(byte threadsCount, byte queueMaxSize)
    {
        if (_started)
            return;

        _started = true;
        _queueMaxSize = queueMaxSize;

        for (var i = 0; i < threadsCount; i++)
        {
            var thread = new Thread(DoWork) { IsBackground = true };
            thread.Start();
        }
    }

    public bool TryEnqueue(BkgTaskContext taskContext)
    {
        lock (_tasksIdentifiers)
        {
            if (_tasksIdentifiers.Contains(taskContext.Identifire))
                return true;

            if (_tasks.Count >= _queueMaxSize)
                return false;
            
            _tasksIdentifiers.Add(taskContext.Identifire);
            _tasks.Add(taskContext);
            return true;
        }
    }

    private void DoWork()
    {
        foreach (var taskContext in _tasks.GetConsumingEnumerable(CancellationToken.None))
        {
            try
            {
                if (_serviceProvider.GetService(taskContext.TaskType) is not IBkgTask service)
                    throw new InvalidCastException(taskContext.TaskType.FullName);

                service.Execute(taskContext).RunSynchronously();
            }
            catch (Exception ex)
            {
                //log
            }
            finally
            {
                lock (_tasksIdentifiers)
                {
                    _tasksIdentifiers.Remove(taskContext.Identifire);
                }
            }
        }
    }
}