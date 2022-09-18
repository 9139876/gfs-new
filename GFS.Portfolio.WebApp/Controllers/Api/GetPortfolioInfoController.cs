using GFS.Portfolio.Api.Interfaces;
using GFS.Portfolio.Api.Models;
using GFS.Portfolio.BL.Services;

namespace GFS.Portfolio.WebApp.Controllers.Api
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