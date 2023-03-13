using GFS.Api.Services;
using GFS.FakeDealer.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.FakeDealer.Api.Interfaces;

[Route(nameof(GetDealerSettings))]
public abstract class GetDealerSettings : ApiServiceWithResponse<DealerSettings>
{
    protected GetDealerSettings(ILogger logger) : base(logger)
    {
    }
}