using GFS.BkgWorker.Enum;
using GFS.QuotesService.Api.Common.Enum;
using GFS.QuotesService.BackgroundWorker.Api.Enum;

namespace GFS.QuotesService.BackgroundWorker.Api.Models;

/// <summary>
/// Задача для BackgroundWorker`а
/// </summary>
public class BkgWorkerTask
{
    public Guid TaskId { get; } = Guid.NewGuid();

    public Guid? AssetId { get; init; }

    public QuotesProviderTypeEnum QuotesProviderType { get; init; }

    public GetQuotesTaskTypeEnum TaskType { get; init; }

    public TaskStateEnum State { get; set; }

    public string? Error { get; set; }

    public int GetPriority() => (int)TaskType;

    public override bool Equals(object? obj)
        => obj is BkgWorkerTask task && task.TaskId == TaskId;

    public override int GetHashCode()
        => TaskId.GetHashCode();
}