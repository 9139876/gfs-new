using GFS.Api.Services;
using GFS.Broker.Api.Models.Portfolio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.Broker.Api.Interfaces.Portfolio
{
    [Route(nameof(Operation))]
    public abstract class Operation : ApiServiceWithRequestResponse<PortfolioOperationRequestDto, OperationResponseDto>
    {
        protected Operation(ILogger logger) : base(logger)
        {
        }
    }
}