using GFS.Api.Services;
using GFS.Broker.Api.Models.Portfolio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.Broker.Api.Interfaces.Portfolio
{
    [Route(nameof(GetPortfolioInfoWithHistory))]
    public abstract class GetPortfolioInfoWithHistory : ApiServiceWithRequestResponse<GetPortfolioInfoRequestDto, PortfolioInfoWithHistoryDto>
    {
        protected GetPortfolioInfoWithHistory(ILogger logger) : base(logger)
        {
        }
    }
}