using GFS.Api.Services;
using GFS.Broker.Api.Models.Portfolio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.Broker.Api.Interfaces.Portfolio
{
    [Route(nameof(GetAllPortfoliosInfo))]
    public abstract class GetAllPortfoliosInfo : ApiServiceWithResponse<List<PortfolioInfoDto>>
    {
        protected GetAllPortfoliosInfo(ILogger<GetAllPortfoliosInfo> logger) : base(logger)
        {
        }
    }
}