using GFS.QuotesService.BackgroundWorker.Api.Interfaces;
using GFS.QuotesService.BackgroundWorker.Api.Models.RequestResponse;
using GFS.QuotesService.BackgroundWorker.Execution;

namespace GFS.QuotesService.BackgroundWorker.Controllers.Api;

public class GetTasksController : GetTasks
{
    public GetTasksController(ILogger logger) : base(logger)
    {
    }

    protected override Task<GetTasksResponse> ExecuteInternal(GetTasksRequest request)
    {
        return Task.FromResult(new GetTasksResponse { Tasks = TasksStorage.GetTasks(request.TaskStates.ToArray()) });
    }
}