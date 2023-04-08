using GFS.Api.Services;
using GFS.Broker.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.Broker.Api.Interfaces;

[Route(nameof(MakeDeal))]
public abstract class MakeDeal : ApiServiceWithRequestResponse<MakeDealRequest, MakeDealResponse>
{
    protected MakeDeal(ILogger logger) : base(logger)
    {
    }
}