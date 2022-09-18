using GFS.Api.Services;
using GFS.Portfolio.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.Portfolio.Api.Interfaces
{
    [Route(nameof(GetAllPortfoliosInfo))]
    public abstract class GetAllPortfoliosInfo : ApiServiceWithResponse<List<PortfolioInfoDto>>
    {
        protected GetAllPortfoliosInfo(ILogger logger) : base(logger)
        {
        }
    }
}