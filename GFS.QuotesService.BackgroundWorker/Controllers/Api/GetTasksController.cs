using GFS.BkgWorker.Task;
using GFS.Common.Exceptions;
using GFS.QuotesService.Api.Common.Enum;
using GFS.QuotesService.BackgroundWorker.Api.Interfaces;
using GFS.QuotesService.BackgroundWorker.Api.Models;
using GFS.QuotesService.BackgroundWorker.TaskContexts;

namespace GFS.QuotesService.BackgroundWorker.Controllers.Api;

public class GetTasksController : GetTasks
{
    public GetTasksController(ILogger logger) : base(logger)
    {
    }

    protected override Task<GetTasksResponse> ExecuteInternal(GetTasksRequest request)
    {
        if (!WorkersManager.TryGetWorker(request.QuotesProviderType, out var worker))
            throw new NotFoundException($"Worker for {request.QuotesProviderType}");

        var tasks = worker!.GetTasks();

        var taskResponses = tasks
            .Select(CreateTaskResponse)
            .ToList();

        return Task.FromResult(new GetTasksResponse { Tasks = taskResponses });
    }

    private static TaskResponse CreateTaskResponse(BackgroundTask task)
    {
        return task switch
        {
            _ when task.Context is GetHistoryQuotesTaskContext getHistoryQuotesTaskContext => CreateTaskResponse(getHistoryQuotesTaskContext, task),
            _ when task.Context is GetInitialDataTaskContext getInitialDataTaskContext => CreateTaskResponse(getInitialDataTaskContext, task),
            _ when task.Context is GetRealtimeQuotesTaskContext getRealtimeQuotesTaskContext => CreateTaskResponse(getRealtimeQuotesTaskContext, task),
            _ => throw new ArgumentOutOfRangeException(nameof(task), task, null)
        };
    }

    private static TaskResponse CreateTaskResponse(GetHistoryQuotesTaskContext taskContext, BackgroundTask backgroundTask)
        => new()
        {
            QuotesProviderType = taskContext.QuotesProviderType,
            TaskType = GetQuotesTaskTypeEnum.GetHistory,
            AssetId = taskContext.AssetId,
            TaskPriority = backgroundTask.Priority,
            TaskState = backgroundTask.State,
            LastError = backgroundTask.LastError
        };

    private static TaskResponse CreateTaskResponse(GetInitialDataTaskContext taskContext, BackgroundTask backgroundTask)
        => new()
        {
            QuotesProviderType = taskContext.QuotesProviderType,
            TaskType = GetQuotesTaskTypeEnum.GetHistory,
            AssetId = null,
            TaskPriority = backgroundTask.Priority,
            TaskState = backgroundTask.State,
            LastError = backgroundTask.LastError
        };

    private static TaskResponse CreateTaskResponse(GetRealtimeQuotesTaskContext taskContext, BackgroundTask backgroundTask)
        => new()
        {
            QuotesProviderType = taskContext.QuotesProviderType,
            TaskType = GetQuotesTaskTypeEnum.GetHistory,
            AssetId = taskContext.AssetId,
            TaskPriority = backgroundTask.Priority,
            TaskState = backgroundTask.State,
            LastError = backgroundTask.LastError
        };
}