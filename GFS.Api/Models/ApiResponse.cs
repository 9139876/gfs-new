using System;
using GFS.Api.Enums;
using Newtonsoft.Json;

namespace GFS.Api.Models
{
    /// <summary>
    /// Wrapper for api response
    /// </summary>
    /// <typeparam name="T">Type of response model</typeparam>
    public class ApiResponse<T> : ApiEmptyResponse
    {
        /// <summary>
        /// Сокрытие конструктора. Для созадния экземпляра нужно пользоваться статическими методами.
        /// </summary>
        protected ApiResponse() : base()
        {
        }

        [JsonProperty]
        public T? Payload { get; private set; }

        public static ApiResponse<T> CreateSuccessResponse(Guid traceId, T payload) =>
            new ApiResponse<T>()
            {
                TraceId = traceId,
                IsSuccess = true,
                Payload = payload
            };

        public new static ApiResponse<T> CreateFailResponse(Guid traceId, Exception exception) =>
            new ApiResponse<T>()
            {
                TraceId = traceId,
                IsSuccess = false,
                Exception = new ApiException(exception)
            };

        public new static ApiResponse<T> CreateRemoteInternalServerErrorResponse(Guid traceId, string? message) =>
            new ApiResponse<T>()
            {
                TraceId = traceId,
                IsSuccess = false,
                Exception = new ApiException(ApiExceptionsTypeEnum.RemoteServerError, message)
            };
    }
}