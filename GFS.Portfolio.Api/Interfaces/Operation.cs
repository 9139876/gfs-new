using GFS.Api.Services;
using GFS.Portfolio.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.Portfolio.Api.Interfaces
{
    [Route(nameof(Operation))]
    public abstract class Operation : ApiServiceWithRequestResponse<OperationRequestDto, OperationResponseDto>
    {
        protected Operation(ILogger logger) : base(logger)
        {
        }
    }
}