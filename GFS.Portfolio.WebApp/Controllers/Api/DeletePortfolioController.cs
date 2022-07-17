using System.Threading.Tasks;
using GFS.Portfolio.Api.Interfaces;
using GFS.Portfolio.Api.Models;
using GFS.Portfolio.BL.Services;
using Microsoft.Extensions.Logging;

namespace GFS.Portfolio.WebApp.Controllers.Api
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