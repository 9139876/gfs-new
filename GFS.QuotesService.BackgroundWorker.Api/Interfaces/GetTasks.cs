using GFS.Api.Services;
using GFS.QuotesService.BackgroundWorker.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.QuotesService.BackgroundWorker.Api.Interfaces;

[Route(nameof(GetTasks))]
public abstract class GetTasks : ApiServiceWithRequestResponse<GetTasksRequest, GetTasksResponse>
{
    protected GetTasks(ILogger logger) : base(logger)
    {
    }
}