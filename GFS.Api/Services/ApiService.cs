using GFS.Api.Models;
using GFS.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.Api.Services
{
    [ApiController]
    public abstract class ApiService
    {
        private readonly ILogger<ApiService> _logger;

        protected ApiService(ILogger<ApiService> logger)
        {
            _logger = logger;
        }
        
        [HttpPost(ApiServiceParameters.PATH)]
        public async Task<ApiEmptyResponse> Execute(ApiEmptyRequest request)
        {
            try
            {
                request.RequiredNotNull(nameof(request));
                
                await ExecuteInternal();
                var response = ApiEmptyResponse.CreateSuccessResponse(request.TraceId);
                _logger.Log(LogLevel.Debug, request.TraceId.GetHashCode(), ApiServiceHelpers.GetSuccessMessage(request), request, response);
                
                return response;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, request.TraceId.GetHashCode(), ex, ApiServiceHelpers.GetFailMessage(request), request);
                return ApiEmptyResponse.CreateFailResponse(request.TraceId, ex);
            }
        }
        
        protected abstract Task ExecuteInternal();
    }
}