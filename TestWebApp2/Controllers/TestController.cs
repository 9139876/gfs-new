using System.Threading.Tasks;
using GFS.Api.Client.Services;
using Microsoft.AspNetCore.Mvc;
using testApi1;

namespace TestWebApp2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IRemoteApiClient _remoteApiClient;

        public TestController(IRemoteApiClient remoteApiClient)
        {
            _remoteApiClient = remoteApiClient;
        }
        
        [HttpPost("test-remote")]
        public async Task<string> Execute(string request)
        {
            var response = await _remoteApiClient.Call<TestApi, string, string>(request);

            return response;
        }
        
        [HttpPost("test-remote-wo")]
        public async Task Execute2()
        {
            await _remoteApiClient.Call<TestApiWithoutRequest>();
        }
    }
}