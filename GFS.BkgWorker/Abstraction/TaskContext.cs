namespace GFS.BkgWorker.Abstraction;

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

    protected abstract string SerializeImpl();
}