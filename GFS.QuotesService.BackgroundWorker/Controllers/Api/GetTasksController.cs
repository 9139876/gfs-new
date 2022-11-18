using GFS.QuotesService.BackgroundWorker.Api.Interfaces;
using GFS.QuotesService.BackgroundWorker.Api.Models;

namespace GFS.QuotesService.BackgroundWorker.Controllers.Api;

public class GetTasksController : GetTasks
{
    public GetTasksController(ILogger logger) : base(logger)
    {
    }

    protected override Task<GetTasksResponse> ExecuteInternal(GetTasksRequest request)
    {
        throw new NotImplementedException();
    }
}