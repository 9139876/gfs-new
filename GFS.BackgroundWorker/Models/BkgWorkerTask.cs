using GFS.BackgroundWorker.Enums;

#pragma warning disable CS8618

namespace GFS.BackgroundWorker.Models;

/// <summary>
/// Задача для BackgroundWorker`а
/// </summary>
public class BkgWorkerTask<TContext>
    where TContext : class, IBkgWorkerTaskContext
{
    public Guid TaskId { get; } = Guid.NewGuid();
    public DateTime CreatedAt { get; } = DateTime.Now;
    public TaskStateEnum State { get; internal set; }
    public string SubState { get; internal set; } = "unknown";
    public DateTime? StateDate { get; internal set; }
    public string? Error { get; internal set; }

    public TContext Context { get; init; }

    public override bool Equals(object? obj)
        => obj is BkgWorkerTask<TContext> task && task.TaskId == TaskId;

    public override int GetHashCode()
        => TaskId.GetHashCode();
}