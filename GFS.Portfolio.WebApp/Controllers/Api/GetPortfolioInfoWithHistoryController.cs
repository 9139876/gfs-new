using System.Threading.Tasks;
using GFS.Portfolio.Api.Interfaces;
using GFS.Portfolio.Api.Models;
using GFS.Portfolio.BL.Services;
using Microsoft.Extensions.Logging;

namespace GFS.Portfolio.WebApp.Controllers.Api
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