using GFS.Api.Services;
using GFS.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.QuotesService.BackgroundWorker.Api.Interfaces;

[Route(nameof(CancelTask))]
public abstract class CancelTask : ApiServiceWithResponse<StandardResponse>
{
    protected CancelTask(ILogger logger) : base(logger)
    {
    }
}