using GFS.BkgWorker.Abstraction;

namespace GFS.BkgWorker;

/// <summary>
/// Класс потокобезопасной очереди заданий с приоритетами без повторов
/// </summary>
/// <typeparam name="TContext">Тип контекста задания</typeparam>
public class PriorityUniqueTaskQueue<TContext>
    where TContext : TaskContext
{
    private readonly TaskWithAttempt<TContext>?[] _queue;
    private readonly Dictionary<PriorityType, Queue<int>> _priorityQueues;
    private readonly object _locker = new();

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="queueSize">Размер очереди</param>
    /// <exception cref="InvalidOperationException">Размер очереди должен быть больше 0</exception>
    public PriorityUniqueTaskQueue(short queueSize)
    {
        if (queueSize <= 0)
            throw new InvalidOperationException($"{nameof(queueSize)} must be more zero");

        _queue = new TaskWithAttempt<TContext>?[queueSize];
        _priorityQueues = new Dictionary<PriorityType, Queue<int>>
        {
            { PriorityType.High, new() },
            { PriorityType.Medium, new() },
            { PriorityType.Low, new() }
        };
    }

    /// <summary>
    /// Попытка поставить задание в очередь с указанным приоритетом 
    /// </summary>
    /// <param name="taskContext">Задание</param>
    /// <param name="priorityType">Приоритет</param>
    /// <param name="attempts">Количество попыток выполнения задания</param>
    /// <returns>Признак успеха постановки задания в очередь</returns>
    /// <remarks>Если задание уже в очереди, вернется признак успеха, но задание не добавится и его приоритет не изменится</remarks>
    public bool TryEnqueue(TContext taskContext, PriorityType priorityType = PriorityType.Medium, byte attempts = 3)
    {
        lock (_locker)
        {
            if (!TryGetEmptySlot(out var index))
                return false;

            var addedTwa = new TaskWithAttempt<TContext>(taskContext, attempts);

            if (_queue.Any(twa => twa != null && twa.Equals(addedTwa)))
                return true;

            _queue[index] = addedTwa;
            _priorityQueues[priorityType].Enqueue(index);
            return true;
        }
    }

    /// <summary>
    /// Попытка получить следующее по приоритету задание на выполнение
    /// </summary>
    /// <param name="taskContext">Задание</param>
    /// <returns>Признак успеха получения задания</returns>
    public bool TryDequeue(out TContext? taskContext)
    {
        lock (_locker)
        {
            var index = 0;
            var taskFound = false;

            foreach (var priorityQueue in _priorityQueues.Values)
            {
                taskFound = priorityQueue.TryDequeue(out index);
                if (taskFound)
                    break;
            }

            taskContext = taskFound ? _queue[index]!.Task : default;
            return taskContext != null && !taskContext.Equals(default);
        }
    }

    public void ReportOfSuccessTaskExecution(TContext taskContext)
    {
        if (taskContext == null)
            throw new InvalidOperationException("Task must be not null");

        lock (_locker)
        {
            var existingTwa = _queue.FirstOrDefault(twa => twa != null && twa.Task.Equals(taskContext));
            if (existingTwa == null)
                throw new InvalidOperationException($"Task {taskContext.Serialize()} not found");

            _queue[Array.IndexOf(_queue, existingTwa)] = null;
        }
    }

    public void ReportOfFailTaskExecution(TContext taskContext)
    {
        if (taskContext == null)
            throw new InvalidOperationException("Task must be not null");

        lock (_locker)
        {
            var existingTwa = _queue.FirstOrDefault(twa => twa != null && twa.Task.Equals(taskContext));
            if (existingTwa == null)
                throw new InvalidOperationException($"Task {taskContext.Serialize()} not found");

            if (existingTwa.AttemptLeft <= 0)
                _queue[Array.IndexOf(_queue, existingTwa)] = null;
            else
            {
                existingTwa.AttemptLeft--;
                _priorityQueues[PriorityType.Low].Enqueue(Array.IndexOf(_queue, existingTwa));
            }
        }
    }

    /// <summary>
    /// Посмотреть все задания в очереди
    /// </summary>
    /// <returns>Массив всех заданий в очереди</returns>
    public TContext[] GetAllTasksInQueue()
    {
        lock (_locker)
        {
            return _queue.Where(twa => twa != null)
                .Select(twa => twa!.Task)
                .ToArray();
        }
    }

    /// <summary>
    /// Посмотреть все задания в очереди с указанным приоритетом
    /// </summary>
    /// <param name="priorityType">Приоритет</param>
    /// <returns>Массив всех заданий в очереди с указанным приоритетом</returns>
    public TContext[] GetPriorityTasksInQueue(PriorityType priorityType)
    {
        lock (_locker)
        {
            return _priorityQueues[priorityType].ToArray().Select(index => _queue[index]!.Task).ToArray();
        }
    }

    /// <summary>
    /// Поиск свободного места в очереди
    /// </summary>
    /// <param name="index">Индекс свободного места в очереди</param>
    /// <returns>Признак наличия свободного места в очереди</returns>
    private bool TryGetEmptySlot(out int index)
    {
        index = Array.IndexOf(_queue, null);
        return index >= 0;
    }
}

/// <summary>
/// Приоритет
/// </summary>
public enum PriorityType
{
    High = 1,
    Medium = 2,
    Low = 3
}

public class TaskWithAttempt<T> : IEquatable<TaskWithAttempt<T>>
    where T : TaskContext
{
    public TaskWithAttempt(T task, byte attemptLeft)
    {
        Task = task ?? throw new InvalidOperationException("Task must be not null");
        AttemptLeft = attemptLeft;
    }

    public T Task { get; }
    public byte AttemptLeft { get; set; }

    public bool Equals(TaskWithAttempt<T>? other)
        => other != null && other.Task.Equals(Task);

    public override bool Equals(object? obj)
        => obj is TaskWithAttempt<T> twa && Equals(twa);

    public override int GetHashCode()
        => Task.GetHashCode();
}