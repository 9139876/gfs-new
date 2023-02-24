namespace GFS.QuotesService.BackgroundWorker.Api.Models.RequestResponse;

public class AddTasksRequest
{
    public List<BkgWorkerTaskCreateRequest> Tasks { get; init; } = new();
}