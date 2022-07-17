using System.Threading.Tasks;
using GFS.Portfolio.Api.Interfaces;
using GFS.Portfolio.Api.Models;
using GFS.Portfolio.BL.Services;
using Microsoft.Extensions.Logging;

namespace GFS.Portfolio.WebApp.Controllers.Api
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

        protected override async Task<OperationResponseDto> ExecuteInternal(OperationRequestDto request)
        {
            return await _portfolioService.PerformOperation(request);
        }
    }
}