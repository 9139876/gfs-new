namespace GFS.QuotesService.BackgroundWorker.Api.Models.RequestResponse;

public class AddTasksRequest
{
    public List<BkgWorkerTask> Tasks { get; init; } = new();
}