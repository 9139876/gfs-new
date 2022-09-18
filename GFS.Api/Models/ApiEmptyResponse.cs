using GFS.Api.Enums;
using Newtonsoft.Json;

namespace GFS.Api.Models
{
    /// <summary>
    /// Wrapper for empty api response
    /// </summary>
    public class ApiEmptyResponse
    {
        /// <summary>
        /// Сокрытие конструктора. Для созадния экземпляра нужно пользоваться статическими методами.
        /// </summary>
        protected ApiEmptyResponse()
        {
        }

        [JsonProperty]
        public Guid TraceId { get; protected set; }

        [JsonProperty]
        public bool IsSuccess { get; protected set; }

        [JsonProperty]
        public ApiException? Exception { get; protected set; }

        public static ApiEmptyResponse CreateSuccessResponse(Guid traceId) =>
            new ApiEmptyResponse()
            {
                TraceId = traceId,
                IsSuccess = true,
            };

        public static ApiEmptyResponse CreateFailResponse(Guid traceId, Exception exception) =>
            new ApiEmptyResponse()
            {
                TraceId = traceId,
                IsSuccess = false,
                Exception = new ApiException(exception)
            };

        public static ApiEmptyResponse CreateRemoteInternalServerErrorResponse(Guid traceId, string? message) =>
            new ApiEmptyResponse()
            {
                TraceId = traceId,
                IsSuccess = false,
                Exception = new ApiException(ApiExceptionsTypeEnum.RemoteServerError, message)
            };
    }
}