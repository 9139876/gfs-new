using GFS.Api.Services;
using GFS.Common.Models;
using GFS.QuotesService.BackgroundWorker.Api.Models.RequestResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.QuotesService.BackgroundWorker.Api.Interfaces;

[Route(nameof(AddTasks))]
public abstract class AddTasks : ApiServiceWithRequestResponse<AddTasksRequest, StandardResponse>
{
    protected AddTasks(ILogger logger) : base(logger)
    {
    }
}