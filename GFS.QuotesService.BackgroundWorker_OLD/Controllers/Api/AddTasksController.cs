using GFS.BackgroundWorker.Execution;
using GFS.BackgroundWorker.Models;
using GFS.Common.Models;
using GFS.QuotesService.BackgroundWorker.Api.Interfaces;
using GFS.QuotesService.BackgroundWorker.Api.Models;
using GFS.QuotesService.BackgroundWorker.Api.Models.RequestResponse;

namespace GFS.QuotesService.BackgroundWorker.Controllers.Api;

public class AddTasksController : AddTasks
{
    private readonly ITasksStorage<QuotesServiceBkgWorkerTaskContext> _tasksStorage;

    public AddTasksController(
        ILogger<AddTasks> logger,
        ITasksStorage<QuotesServiceBkgWorkerTaskContext> tasksStorage) : base(logger)
    {
        _tasksStorage = tasksStorage;
    }

    protected override Task<StandardResponse> ExecuteInternal(AddTasksRequest request)
    {
        _tasksStorage.AddTasks(request.Tasks.Select(taskContext => new BkgWorkerTask<QuotesServiceBkgWorkerTaskContext> { Context = taskContext }));
        return Task.FromResult(StandardResponse.GetSuccessResponse());
    }
}