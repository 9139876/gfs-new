using GFS.QuotesService.BackgroundWorker.Api.Enum;

namespace GFS.QuotesService.BackgroundWorker.Api.Models.RequestResponse;

public class GetTasksRequest
{
    public List<TaskStateEnum> TaskStates { get; init; } = new();
}