using GFS.Api.Services;
using GFS.Broker.Api.Models.Portfolio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.Broker.Api.Interfaces.Portfolio
{
    [Route(nameof(GetPortfolioInfo))]
    public abstract class GetPortfolioInfo : ApiServiceWithRequestResponse<GetPortfolioInfoRequestDto, PortfolioInfoDto>
    {
        protected GetPortfolioInfo(ILogger logger) : base(logger)
        {
        }
    }
}