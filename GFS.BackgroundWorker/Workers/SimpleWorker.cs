using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GFS.BackgroundWorker.Workers;

public abstract class SimpleWorker<TTaskData>
    where TTaskData : class, ILoggingSerializable
{
    private readonly IServiceProvider _serviceProvider;
    protected ILogger<SimpleWorker<TTaskData>> Logger;

    protected SimpleWorker(IServiceProvider serviceProvider, ILogger<SimpleWorker<TTaskData>> logger)
    {
        _serviceProvider = serviceProvider;
        Logger = logger;
    }

    public async Task DoWork()
    {
        while (true)
        {
            TTaskData? currentData = null;

            try
            {
                using var scope = _serviceProvider.CreateScope();

                Logger.LogInformation("Получение данных для итерации");
                var tasksData = await GetTasksData(scope.ServiceProvider);
                Logger.LogInformation("Получено {tasksCount} элементов", tasksData.Count);

                while (tasksData.Any())
                {
                    var taskDataItem = tasksData[0];

                    Logger.LogInformation("Обработка задачи: {task}", taskDataItem.Serialize());
                    currentData = taskDataItem;
                    var (message, newTaskData) = await DoTaskInternal(scope.ServiceProvider, taskDataItem);

                    if (newTaskData != null)
                    {
                        tasksData.Add(newTaskData);
                        Logger.LogInformation("Добавлена новая задача в конец очереди - {newTask}", newTaskData.Serialize());
                    }

                    tasksData.RemoveAt(0);
                    Logger.LogInformation("Обработка завершена - {result}", message);
                }

                Thread.Sleep(SleepTime);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Ошибка воркера {error} при выполнении '{task}'", ex.Message, currentData?.Serialize());
            }
        }
    }

    protected abstract TimeSpan SleepTime { get; }
    
    protected abstract Task<List<TTaskData>> GetTasksData(IServiceProvider serviceProvider);

    protected abstract Task<TaskExecutingResult<TTaskData>> DoTaskInternal(IServiceProvider serviceProvider, TTaskData taskDataItem);
}

public interface ILoggingSerializable
{
    string Serialize();
}

public record TaskExecutingResult<TTaskData>(string Message, TTaskData? NewTaskData) where TTaskData : class, ILoggingSerializable;