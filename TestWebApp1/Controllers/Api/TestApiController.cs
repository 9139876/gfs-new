using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using testApi1;

namespace TestWebApp1.Controllers.Api
{
    public class TestApiController : TestApi
    {
        public TestApiController(ILogger logger) : base(logger)
        {
        }
        
        protected override async Task<string> ExecuteInternal(string request)
        {
            return await Task.FromResult($"Hello world from {request}");
        }
    }
}