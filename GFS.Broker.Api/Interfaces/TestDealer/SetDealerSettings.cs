using GFS.Api.Services;
using GFS.Broker.Api.Models.TestDealer;
using GFS.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.Broker.Api.Interfaces.TestDealer;

[Route(nameof(SetDealerSettings))]
public abstract class SetDealerSettings : ApiServiceWithRequestResponse<DealerSettings, StandardResponse>
{
    protected SetDealerSettings(ILogger<SetDealerSettings> logger) : base(logger)
    {
    }
}