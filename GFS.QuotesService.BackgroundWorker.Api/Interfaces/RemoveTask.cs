using GFS.Api.Services;
using GFS.Common.Models;
using GFS.QuotesService.BackgroundWorker.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.QuotesService.BackgroundWorker.Api.Interfaces;

[Route(nameof(RemoveTask))]
public abstract class RemoveTask : ApiServiceWithRequestResponse<AddTaskRequest, StandardResponse>
{
    protected RemoveTask(ILogger logger) : base(logger)
    {
    }
}