using GFS.Api.Services;
using GFS.Portfolio.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.Portfolio.Api.Interfaces
{
    [Route(nameof(GetPortfolioInfo))]
    public abstract class GetPortfolioInfo : ApiServiceWithRequestResponse<GetPortfolioInfoRequestDto, PortfolioInfoDto>
    {
        protected GetPortfolioInfo(ILogger logger) : base(logger)
        {
        }
    }
}