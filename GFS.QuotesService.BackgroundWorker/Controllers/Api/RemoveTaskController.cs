using GFS.Common.Models;
using GFS.QuotesService.BackgroundWorker.Api.Interfaces;
using GFS.QuotesService.BackgroundWorker.Api.Models;

namespace GFS.QuotesService.BackgroundWorker.Controllers.Api;

public class RemoveTaskController : RemoveTask
{
    public RemoveTaskController(ILogger logger) : base(logger)
    {
    }

    protected override Task<StandardResponse> ExecuteInternal(AddTaskRequest request)
    {
        throw new NotImplementedException();
    }
}