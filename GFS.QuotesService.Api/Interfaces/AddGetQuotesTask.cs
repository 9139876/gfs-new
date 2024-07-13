using GFS.Api.Services;
using GFS.QuotesService.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.QuotesService.Api.Interfaces;

[Route(nameof(AddGetQuotesTask))]
public abstract class AddGetQuotesTask: ApiServiceWithRequest<AddGetQuotesTaskRequest>
{
    public AddGetQuotesTask(ILogger<ApiServiceWithRequest<AddGetQuotesTaskRequest>> logger) : base(logger)
    {
    }
}