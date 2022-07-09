using System;
using GFS.Api.Enums;
using GFS.Common.Exceptions;
using Newtonsoft.Json;

namespace GFS.Api.Models
{
    public class ApiException
    {
        //For json serializer
        public ApiException()
        {
        }
        
        internal ApiException(Exception exception)
        {
            Message = exception.Message;
            ExceptionTypeType = GetApiExceptionType(exception);
        }

        internal ApiException(ApiExceptionsTypeEnum exceptionTypeType, string? message)
        {
            Message = message;
            ExceptionTypeType = exceptionTypeType;
        }

        [JsonProperty]
        public ApiExceptionsTypeEnum ExceptionTypeType { get; private set; }

        [JsonProperty]
        public string? Message { get; private set; }

        private static ApiExceptionsTypeEnum GetApiExceptionType(Exception exception) =>
            exception switch
            {
                { } and NotFoundException => ApiExceptionsTypeEnum.NotFound,
                { } and SingleException => ApiExceptionsTypeEnum.SingleViolation,
                { } and InvalidOperationException => ApiExceptionsTypeEnum.InvalidOperation,
                { } and NotImplementedException => ApiExceptionsTypeEnum.NotImplemented,

                _ => ApiExceptionsTypeEnum.Unknown
            };
    }
}