using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TestWebApp1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger _logger;

        public TestController(ILogger logger)
        {
            _logger = logger;
        }
        
        [HttpGet]
        public string GetHelloWord()
        {
            //_logger.Information("Hi!!!");
            _logger.Log(LogLevel.Information, "Hi!!!");
            return "Hello world!";
        }
    }
}