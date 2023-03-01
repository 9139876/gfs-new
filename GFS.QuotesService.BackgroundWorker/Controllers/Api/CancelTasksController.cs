using GFS.Common.Models;
using GFS.QuotesService.BackgroundWorker.Api.Interfaces;
using GFS.QuotesService.BackgroundWorker.Execution;

namespace GFS.QuotesService.BackgroundWorker.Controllers.Api;

public class CancelTasksController : CancelTasks
{
    public CancelTasksController(ILogger logger) : base(logger)
    {
    }

    protected override Task<StandardResponse> ExecuteInternal()
    {
        TasksStorage.CancelPendingExecutionTasks();
        return Task.FromResult(StandardResponse.GetSuccessResponse());
    }
}