using GFS.Api.Services;
using GFS.FakeDealer.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.FakeDealer.Api.Interfaces;

[Route(nameof(MakeDeal))]
public abstract class MakeDeal : ApiServiceWithRequestResponse<MakeDealRequest, MakeDealResponse>
{
    protected MakeDeal(ILogger logger) : base(logger)
    {
    }
}