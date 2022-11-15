using GFS.Api.Services;
using GFS.Common.Models;
using GFS.QuotesService.BackgroundWorker.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.QuotesService.BackgroundWorker.Api.Interfaces;

[Route(nameof(AddTask))]
public abstract class AddTask : ApiServiceWithRequestResponse<AddTaskRequest, StandardResponse>
{
    protected AddTask(ILogger logger) : base(logger)
    {
    }
}