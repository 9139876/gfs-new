using GFS.Api.Models;
using GFS.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.Api.Services
{
    [ApiController]
    public abstract class ApiServiceWithRequestResponse<TRequestPayload, TResponsePayload> : ControllerBase
    {
        private readonly ILogger _logger;

        protected ApiServiceWithRequestResponse(ILogger logger)
        {
            _logger = logger;
        }

        [HttpPost(ApiServiceParameters.PATH)]
        public async Task<ApiResponse<TResponsePayload>> Execute([FromBody] ApiRequest<TRequestPayload> request)
        {
            try
            {
                request.RequiredNotNull(nameof(request));
                request.Payload.RequiredNotNull(nameof(request.Payload));
                
                var response = ApiResponse<TResponsePayload>.CreateSuccessResponse(request.TraceId, await ExecuteInternal(request.Payload));
                _logger.Log(LogLevel.Debug, request.TraceId.GetHashCode(), ApiServiceHelpers.GetSuccessMessage(request), request, response);

                return response;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, request.TraceId.GetHashCode(), ex, ApiServiceHelpers.GetFailMessage(request), request);
                return ApiResponse<TResponsePayload>.CreateFailResponse(request.TraceId, ex);
            }
        }

        protected abstract Task<TResponsePayload> ExecuteInternal(TRequestPayload request);
    }
}