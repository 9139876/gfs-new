using GFS.Portfolio.Api.Interfaces;
using GFS.Portfolio.Api.Models;
using GFS.Portfolio.BL.Services;

namespace GFS.Portfolio.WebApp.Controllers.Api
{
    public class GetAllPortfoliosInfoController : GetAllPortfoliosInfo
    {
        private readonly IPortfolioService _portfolioService;

        public GetAllPortfoliosInfoController(
            ILogger logger,
            IPortfolioService portfolioService) : base(logger)
        {
            _portfolioService = portfolioService;
        }

        protected override async Task<List<PortfolioInfoDto>> ExecuteInternal()
        {
            return await _portfolioService.GetAllPortfoliosInfo();
        }
    }
}