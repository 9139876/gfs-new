using GFS.BkgWorker.Task;
using GFS.Common.Exceptions;
using GFS.Common.Models;
using GFS.QuotesService.BackgroundWorker.Api.Interfaces;
using GFS.QuotesService.BackgroundWorker.Api.Models;
using GFS.QuotesService.BackgroundWorker.TaskContexts;

namespace GFS.QuotesService.BackgroundWorker.Controllers.Api;

public class CancelTaskController : CancelTask
{
    public CancelTaskController(ILogger logger) : base(logger)
    {
    }

    protected override Task<StandardResponse> ExecuteInternal(CancelTaskRequest request)
    {
        if (!WorkersManager.TryGetWorker(request.QuotesProviderType, out var worker))
            throw new NotFoundException($"Worker for {request.QuotesProviderType}");

        var backgroundTask = new BackgroundTask(TaskContextFactory.CreateTaskContext(request.TaskType,request.QuotesProviderType, request.AssetId, request.TimeFrame));
        
        worker!.CancelTask(backgroundTask);
        return Task.FromResult(StandardResponse.GetSuccessResponse());
    }
}