using GFS.Broker.Api.Interfaces.Portfolio;
using GFS.Broker.Api.Models.Portfolio;
using GFS.Broker.BL.Services;

namespace GFS.Broker.WebApp.Controllers.Api.Portfolio
{
    public class DeletePortfolioController : DeletePortfolio
    {
        private readonly IPortfolioService _portfolioService;

        public DeletePortfolioController(
            ILogger logger,
            IPortfolioService portfolioService) : base(logger)
        {
            _portfolioService = portfolioService;
        }

        protected override async Task ExecuteInternal(DeletePortfolioRequestDto request)
        {
            await _portfolioService.DeletePortfolio(request);
        }
    }
}