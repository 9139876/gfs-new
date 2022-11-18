namespace GFS.BkgWorker.Task;

public abstract class TaskContext : IEquatable<TaskContext>
{
    public string Serialize()
        => $"{GetType().FullName} {SerializeImpl()}";

    public bool Equals(TaskContext? other)
        => other != null && other.Serialize() == Serialize();

    public override bool Equals(object? obj)
        => Equals(obj as TaskContext);

    public override int GetHashCode()
        => Serialize().GetHashCode();

    /// <summary>
    /// Выполняемая работа
    /// </summary>
    /// <param name="serviceProvider">Service Provider</param>
    /// <returns>Признак, что задача не выполнена до конца и нужна еще итерация</returns>
    public abstract Task<bool> DoWork(IServiceProvider serviceProvider);
    
    protected abstract string SerializeImpl();
}