using GFS.Api.Services;
using GFS.Broker.Api.Models.TestDealer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.Broker.Api.Interfaces.TestDealer;

[Route(nameof(GetDealerSettings))]
public abstract class GetDealerSettings : ApiServiceWithResponse<DealerSettings>
{
    protected GetDealerSettings(ILogger<GetDealerSettings> logger) : base(logger)
    {
    }
}