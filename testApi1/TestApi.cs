using GFS.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace testApi1
{
    [Route(nameof(TestApi))]
    public abstract class TestApi : ApiServiceWithRequestResponse<string, string>
    {
        protected TestApi(ILogger logger) : base(logger)
        {
        }
    }
    
    [Route(nameof(TestApiWithoutRequest))]
    public abstract class TestApiWithoutRequest : ApiService
    {
        protected TestApiWithoutRequest(ILogger logger) : base(logger)
        {
        }
    }
}