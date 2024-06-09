using GFS.Api.Services;
using GFS.Broker.Api.Models.Portfolio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.Broker.Api.Interfaces.Portfolio
{
    [Route(nameof(PortfolioDepositOperation))]
    public abstract class PortfolioDepositOperation : ApiServiceWithRequestResponse<PortfolioDepositOperationRequestDto, PortfolioOperationResponseDto>
    {
        protected PortfolioDepositOperation(ILogger<PortfolioDepositOperation> logger) : base(logger)
        {
        }
    }
}