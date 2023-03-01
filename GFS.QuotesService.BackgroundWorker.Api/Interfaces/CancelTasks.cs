using GFS.Api.Services;
using GFS.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.QuotesService.BackgroundWorker.Api.Interfaces;

[Route(nameof(CancelTasks))]
public abstract class CancelTasks : ApiServiceWithResponse<StandardResponse>
{
    protected CancelTasks(ILogger logger) : base(logger)
    {
    }
}