using System.Threading.Tasks;
using GFS.Portfolio.Api.Interfaces;
using GFS.Portfolio.Api.Models;
using GFS.Portfolio.BL.Services;
using Microsoft.Extensions.Logging;

namespace GFS.Portfolio.WebApp.Controllers.Api
{
    public class CreatePortfolioController : CreatePortfolio
    {
        private readonly IPortfolioService _portfolioService;

        public CreatePortfolioController(
            ILogger logger,
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