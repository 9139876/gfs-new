using GFS.BackgroundWorker.Execution;
using GFS.Common.Models;
using GFS.QuotesService.BackgroundWorker.Api.Interfaces;
using GFS.QuotesService.BackgroundWorker.Api.Models;

namespace GFS.QuotesService.BackgroundWorker.Controllers.Api;

public class CancelTasksController : CancelTasks
{
    private readonly ITasksStorage<QuotesServiceBkgWorkerTaskContext> _tasksStorage;

    public CancelTasksController(
        ILogger logger,
        ITasksStorage<QuotesServiceBkgWorkerTaskContext> tasksStorage) : base(logger)
    {
        _tasksStorage = tasksStorage;
    }

    protected override Task<StandardResponse> ExecuteInternal()
    {
        _tasksStorage.CancelPendingExecutionTasks();
        return Task.FromResult(StandardResponse.GetSuccessResponse());
    }
}