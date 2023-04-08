using GFS.Broker.Api.Interfaces.Portfolio;
using GFS.Broker.Api.Models.Portfolio;
using GFS.Broker.BL.Services;

namespace GFS.Broker.WebApp.Controllers.Api.Portfolio
{
    public class GetPortfolioInfoWithHistoryController : GetPortfolioInfoWithHistory
    {
        private readonly IPortfolioService _portfolioService;

        public GetPortfolioInfoWithHistoryController(
            ILogger logger,
            IPortfolioService portfolioService) : base(logger)
        {
            _portfolioService = portfolioService;
        }

        protected override async Task<PortfolioInfoWithHistoryDto> ExecuteInternal(GetPortfolioInfoRequestDto request)
        {
            return await _portfolioService.GetPortfolioInfoWithHistory(request);
        }
    }
}