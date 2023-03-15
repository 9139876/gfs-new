using GFS.BackgroundWorker.Models;

namespace GFS.QuotesService.BackgroundWorker.Api.Models.RequestResponse;

public class GetTasksResponse
{
    public List<BkgWorkerTask<QuotesServiceBkgWorkerTaskContext>> Tasks { get; init; } = new();
}