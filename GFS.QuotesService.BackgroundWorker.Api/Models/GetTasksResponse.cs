using GFS.BkgWorker.Enum;
using GFS.QuotesService.Api.Common.Enum;

namespace GFS.QuotesService.BackgroundWorker.Api.Models;

public class GetTasksResponse
{
    public List<TaskResponse> Tasks { get; set; } = new();
}

public class TaskResponse
{
    public QuotesProviderTypeEnum QuotesProviderType { get; set; }

    public GetQuotesTaskTypeEnum TaskType { get; set; }

    public Guid? AssetId { get; set; }

    public TaskPriorityEnum TaskPriority { get; set; }

    public TaskStateEnum TaskState { get; set; }
    
    public string? LastError { get; set; }
}