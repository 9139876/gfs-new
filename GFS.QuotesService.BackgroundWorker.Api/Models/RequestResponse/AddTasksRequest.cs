namespace GFS.QuotesService.BackgroundWorker.Api.Models.RequestResponse;

public class AddTasksRequest
{
    public List<QuotesServiceBkgWorkerTaskContext> Tasks { get; init; } = new();
}