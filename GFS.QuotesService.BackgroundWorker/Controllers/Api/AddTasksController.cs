using GFS.Common.Models;
using GFS.QuotesService.BackgroundWorker.Api.Interfaces;
using GFS.QuotesService.BackgroundWorker.Api.Models.RequestResponse;
using GFS.QuotesService.BackgroundWorker.Execution;

namespace GFS.QuotesService.BackgroundWorker.Controllers.Api;

public class AddTasksController : AddTasks
{
    public AddTasksController(ILogger logger) : base(logger)
    {
    }

    protected override Task<StandardResponse> ExecuteInternal(AddTasksRequest request)
    {
        TasksStorage.AddTasks(request.Tasks);
        return Task.FromResult(StandardResponse.GetSuccessResponse());
    }
}