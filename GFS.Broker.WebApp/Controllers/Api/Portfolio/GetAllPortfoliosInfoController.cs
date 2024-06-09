using GFS.Broker.Api.Interfaces.Portfolio;
using GFS.Broker.Api.Models.Portfolio;
using GFS.Broker.BL.Services;

namespace GFS.Broker.WebApp.Controllers.Api.Portfolio
{
    public class GetAllPortfoliosInfoController : GetAllPortfoliosInfo
    {
        private readonly IPortfolioService _portfolioService;

        public GetAllPortfoliosInfoController(
            ILogger<GetAllPortfoliosInfo> logger,
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