using GFS.Api.Services;
using GFS.Portfolio.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.Portfolio.Api.Interfaces
{
    [Route(nameof(GetPortfolioInfoWithHistory))]
    public abstract class GetPortfolioInfoWithHistory : ApiServiceWithRequestResponse<GetPortfolioInfoRequestDto, PortfolioInfoWithHistoryDto>
    {
        protected GetPortfolioInfoWithHistory(ILogger logger) : base(logger)
        {
        }
    }
}