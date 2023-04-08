using GFS.Broker.Api.Interfaces.Portfolio;
using GFS.Broker.Api.Models.Portfolio;
using GFS.Broker.BL.Services;

namespace GFS.Broker.WebApp.Controllers.Api.Portfolio
{
    public class GetPortfolioInfoController : GetPortfolioInfo
    {
        private readonly IPortfolioService _portfolioService;

        public GetPortfolioInfoController(
            ILogger logger,
            IPortfolioService portfolioService) : base(logger)
        {
            _portfolioService = portfolioService;
        }

        protected override async Task<PortfolioInfoDto> ExecuteInternal(GetPortfolioInfoRequestDto request)
        {
            return await _portfolioService.GetPortfolioInfo(request);
        }
    }
}