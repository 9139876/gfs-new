using GFS.BkgWorker.Enum;

namespace GFS.QuotesService.BackgroundWorker.Api.Models.RequestResponse;

public class GetTasksRequest
{
    public List<TaskStateEnum> TaskStates { get; init; } = new();
}