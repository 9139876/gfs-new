using GFS.BkgWorker.Enum;
using GFS.BkgWorker.Task;
using GFS.Common.Exceptions;
using GFS.Common.Models;
using GFS.QuotesService.Api.Common.Enum;
using GFS.QuotesService.BackgroundWorker.Api.Interfaces;
using GFS.QuotesService.BackgroundWorker.Api.Models;
using GFS.QuotesService.BackgroundWorker.TaskContexts;

namespace GFS.QuotesService.BackgroundWorker.Controllers.Api;

public class AddTaskController : AddTask
{
    public AddTaskController(ILogger logger) : base(logger)
    {
    }

    protected override Task<StandardResponse> ExecuteInternal(AddTaskRequest request)
    {
        if (WorkersManager.TryGetWorker(request.QuotesProviderType, out var worker))
            throw new NotFoundException($"Not found worker for {request.QuotesProviderType}");

        var backgroundTask = new BackgroundTask(
            TaskContextFactory.CreateTaskContext(
                request.TaskType,
                request.QuotesProviderType,
                request.AssetId,
                request.TimeFrame),
            request.Attempts);

        worker!.EnqueueTask(backgroundTask, GetTaskPriority(request.TaskType));
        return Task.FromResult(StandardResponse.GetSuccessResponse());
    }

    private static TaskPriorityEnum GetTaskPriority(GetQuotesTaskTypeEnum taskType)
        => taskType switch
        {
            GetQuotesTaskTypeEnum.GetRealtimeQuotes => TaskPriorityEnum.High,
            GetQuotesTaskTypeEnum.GetInitialData => TaskPriorityEnum.Medium,
            GetQuotesTaskTypeEnum.GetHistory => TaskPriorityEnum.Low,
            _ => throw new ArgumentOutOfRangeException(nameof(taskType), taskType, null)
        };
}