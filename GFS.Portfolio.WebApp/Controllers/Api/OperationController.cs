using System.Threading.Tasks;
using GFS.Portfolio.Api.Interfaces;
using GFS.Portfolio.Api.Models;
using Microsoft.Extensions.Logging;

namespace GFS.Portfolio.WebApp.Controllers.Api
{
    public class OperationController : Operation
    {
        public OperationController(ILogger logger) : base(logger)
        {
        }

        protected override Task<OperationResponseDto> ExecuteInternal(OperationRequestDto request)
        {
            throw new System.NotImplementedException();
        }
    }
}