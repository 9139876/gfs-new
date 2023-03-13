using GFS.Api.Services;
using GFS.Common.Models;
using GFS.FakeDealer.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.FakeDealer.Api.Interfaces;

[Route(nameof(SetDealerSettings))]
public abstract class SetDealerSettings : ApiServiceWithRequestResponse<DealerSettings, StandardResponse>
{
    protected SetDealerSettings(ILogger logger) : base(logger)
    {
    }
}