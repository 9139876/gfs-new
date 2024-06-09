using GFS.Api.Services;
using GFS.QuotesService.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.QuotesService.Api.Interfaces;

[Route(nameof(GetQuotes))]
public abstract class GetQuotes : ApiServiceWithRequestResponse<GetQuotesRequest, GetQuotesResponse>
{
    protected GetQuotes(ILogger<GetQuotes> logger) : base(logger)
    {
    }
}