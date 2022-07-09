using GFS.Api.Models;

namespace GFS.Api.Services
{
    public static class ApiServiceParameters
    {
        public const string PATH = "execute";
    }

    public static class ApiServiceHelpers
    {
        public static string GetSuccessMessage(ApiEmptyRequest request)
            => $"Api request {request.TraceId} successfully processed";

        public static string GetFailMessage(ApiEmptyRequest request)
            => $"Api request {request.TraceId} processed with error";
    }
}