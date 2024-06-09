using GFS.Broker.Api.Interfaces.Portfolio;
using GFS.Broker.Api.Models.Portfolio;
using GFS.Broker.BL.Services;

namespace GFS.Broker.WebApp.Controllers.Api.Portfolio
{
    public class CreatePortfolioController : CreatePortfolio
    {
        private readonly IPortfolioService _portfolioService;

        public CreatePortfolioController(
            ILogger<CreatePortfolio> logger,
            IPortfolioService portfolioService) : base(logger)
        {
            _portfolioService = portfolioService;
        }

        protected override async Task<PortfolioInfoDto> ExecuteInternal(CreatePortfolioRequestDto request)
        {
            return await _portfolioService.CreatePortfolio(request);
        }
    }
}