using GFS.BkgWorker;
using GFS.BkgWorker.Enum;
using GFS.Common.Models;
using GFS.QuotesService.Api.Common.Enum;
using GFS.QuotesService.BackgroundWorker.Api.Interfaces;
using GFS.QuotesService.BackgroundWorker.Api.Models;

namespace GFS.QuotesService.BackgroundWorker.Controllers.Api;

public class AddTaskController : AddTask
{
    public AddTaskController(ILogger logger) : base(logger)
    {
    }

    protected override Task<StandardResponse> ExecuteInternal(AddTaskRequest request)
    {
        throw new NotImplementedException();
    }

    private static TaskPriorityEnum GetTaskPriority(GetQuotesTaskTypeEnum taskType)
        => taskType switch
        {
            GetQuotesTaskTypeEnum.GetRealtimeQuotes => TaskPriorityEnum.High,
            GetQuotesTaskTypeEnum.GetInitialData => TaskPriorityEnum.Medium,
            GetQuotesTaskTypeEnum.GetHistory => TaskPriorityEnum.Low
        };
}