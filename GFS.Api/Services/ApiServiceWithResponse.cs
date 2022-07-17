using System;
using System.Threading.Tasks;
using GFS.Api.Models;
using GFS.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.Api.Services
{
    [ApiController]
    public abstract class ApiServiceWithResponse<TResponsePayload>
    {
        private readonly ILogger _logger;

        protected ApiServiceWithResponse(ILogger logger)
        {
            _logger = logger;
        }

        [HttpPost(ApiServiceParameters.PATH)]
        public async Task<ApiResponse<TResponsePayload>> Execute(ApiEmptyRequest request)
        {
            try
            {
                request.RequiredNotNull(nameof(request));
                
                var response = ApiResponse<TResponsePayload>.CreateSuccessResponse(request.TraceId, await ExecuteInternal());
                _logger.Log(LogLevel.Debug, request.TraceId.GetHashCode(), ApiServiceHelpers.GetSuccessMessage(request), request, response);

                return response;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, request.TraceId.GetHashCode(), ex, ApiServiceHelpers.GetFailMessage(request), request);
                return ApiResponse<TResponsePayload>.CreateFailResponse(request.TraceId, ex);
            }
        }

        protected abstract Task<TResponsePayload> ExecuteInternal();
    }
}