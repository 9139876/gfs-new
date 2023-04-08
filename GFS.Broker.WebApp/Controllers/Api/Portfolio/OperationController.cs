using GFS.Broker.Api.Interfaces.Portfolio;
using GFS.Broker.Api.Models.Portfolio;
using GFS.Broker.BL.Services;

namespace GFS.Broker.WebApp.Controllers.Api.Portfolio
{
    public class OperationController : Operation
    {
        private readonly IPortfolioService _portfolioService;

        public OperationController(
            ILogger logger,
            IPortfolioService portfolioService) : base(logger)
        {
            _portfolioService = portfolioService;
        }

        protected override async Task<OperationResponseDto> ExecuteInternal(PortfolioOperationRequestDto request)
        {
            return await _portfolioService.PerformOperation(request);
        }
    }
}