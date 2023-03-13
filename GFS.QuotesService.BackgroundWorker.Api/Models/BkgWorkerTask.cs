using GFS.QuotesService.BackgroundWorker.Api.Enum;

namespace GFS.QuotesService.BackgroundWorker.Api.Models;

/// <summary>
/// Задача для BackgroundWorker`а
/// </summary>
public class BkgWorkerTask : BkgWorkerTaskCreateRequest
{
    public Guid TaskId { get; } = Guid.NewGuid();
    public DateTime Created { get; set; }
    public TaskStateEnum State { get; set; }
    public DateTime? StateDate { get; set; }
    public string? Error { get; set; }


    public int GetPriority() => (int)TaskType;

    public override bool Equals(object? obj)
        => obj is BkgWorkerTask task && task.TaskId == TaskId;

    public override int GetHashCode()
        => TaskId.GetHashCode();
}