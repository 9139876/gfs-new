using GFS.BackgroundWorker.Execution;
using GFS.QuotesService.BackgroundWorker.Api.Interfaces;
using GFS.QuotesService.BackgroundWorker.Api.Models;
using GFS.QuotesService.BackgroundWorker.Api.Models.RequestResponse;

namespace GFS.QuotesService.BackgroundWorker.Controllers.Api;

public class GetTasksController : GetTasks
{
    private readonly ITasksStorage<QuotesServiceBkgWorkerTaskContext> _tasksStorage;

    public GetTasksController(
        ILogger<GetTasks> logger,
        ITasksStorage<QuotesServiceBkgWorkerTaskContext> tasksStorage) : base(logger)
    {
        _tasksStorage = tasksStorage;
    }

    protected override Task<GetTasksResponse> ExecuteInternal(GetTasksRequest request)
    {
        return Task.FromResult(new GetTasksResponse { Tasks = _tasksStorage.GetTasks(request.TaskStates.ToArray()) });
    }
}