using GFS.Broker.Api.Interfaces.Portfolio;
using GFS.Broker.Api.Models.Portfolio;
using GFS.Broker.BL.Services;

namespace GFS.Broker.WebApp.Controllers.Api.Portfolio
{
    public class PortfolioDepositOperationController : PortfolioDepositOperation
    {
        private readonly IPortfolioService _portfolioService;

        public PortfolioDepositOperationController(
            ILogger<PortfolioDepositOperation> logger,
            IPortfolioService portfolioService) : base(logger)
        {
            _portfolioService = portfolioService;
        }

        protected override async Task<PortfolioOperationResponseDto> ExecuteInternal(PortfolioDepositOperationRequestDto request)
        {
            return await _portfolioService.PerformDepositOperation(request);
        }
    }
}