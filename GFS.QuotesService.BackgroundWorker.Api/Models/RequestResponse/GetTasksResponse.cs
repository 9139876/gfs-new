namespace GFS.QuotesService.BackgroundWorker.Api.Models.RequestResponse;

public class GetTasksResponse
{
    public List<BkgWorkerTask> Tasks { get; init; } = new();
}