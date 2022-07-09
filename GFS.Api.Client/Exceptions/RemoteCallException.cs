using System;
using GFS.Api.Enums;
using GFS.Api.Models;

namespace GFS.Api.Client.Exceptions
{
    public class RemoteCallException : Exception
    {
        private RemoteCallException(string message) : base(message)
        {
        }

        public static RemoteCallException Create(ApiException? apiException)
            => new RemoteCallException($"Remote server threw an exception - {apiException?.ExceptionTypeType} message '{apiException?.Message}'");
        
        public static RemoteCallException Create(ApiExceptionsTypeEnum apiExceptionType, string? message)
            => new RemoteCallException($"Remote server threw an exception - {apiExceptionType} message '{message}'");
    }
}