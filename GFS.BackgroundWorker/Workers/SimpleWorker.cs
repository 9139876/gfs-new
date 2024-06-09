using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GFS.BackgroundWorker.Workers;

public abstract class SimpleWorker<TTaskData>
    where TTaskData : class, ILoggingSerializable
{
    private readonly IServiceProvider _serviceProvider;
    protected ILogger<SimpleWorker<TTaskData>> Logger;

    protected SimpleWorker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        Logger = _serviceProvider.GetRequiredService<ILogger<SimpleWorker<TTaskData>>>();
    }

    public async Task DoWork(TimeSpan? sleepInSeconds = null, CancellationToken? cancellationToken = null)
    {
        while (cancellationToken?.IsCancellationRequested != true)
        {
            TTaskData? currentData = null;

            try
            {
                using var scope = _serviceProvider.CreateScope();

                Logger.LogInformation("Получение данных для итерации");
                var tasksData = await GetTasksData(scope.ServiceProvider);
                Logger.LogInformation("Получено {count} элементов", tasksData.Count);

                while (tasksData.Any())
                {
                    var taskDataItem = tasksData[0];

                    Logger.LogInformation("Обработка {item}", taskDataItem.Serialize());
                    currentData = taskDataItem;
                    var (message, newTaskData) = await DoTaskAndReturnLoggingText(scope.ServiceProvider, taskDataItem);

                    if (newTaskData != null)
                    {
                        tasksData.Add(newTaskData);
                        Logger.LogInformation("Добавлена новая задача в конец очереди - {newItem}", newTaskData.Serialize());
                    }

                    tasksData.RemoveAt(0);
                    Logger.LogInformation("Обработка завершена - {result}", message);
                }

                foreach (var taskDataItem in tasksData)
                {
                }

                Thread.Sleep(sleepInSeconds ?? TimeSpan.FromMinutes(1));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Ошибка при выполнении воркера {error} при выполнении '{data}'", ex.Message, currentData?.Serialize());
            }
        }
    }

    protected abstract Task<List<TTaskData>> GetTasksData(IServiceProvider serviceProvider);

    protected abstract Task<TaskExecutingResult<TTaskData>> DoTaskAndReturnLoggingText(IServiceProvider serviceProvider, TTaskData taskDataItem);
}

public interface ILoggingSerializable
{
    string Serialize();
}

public record TaskExecutingResult<TTaskData>(string Message, TTaskData? NewTaskData) where TTaskData : class, ILoggingSerializable;