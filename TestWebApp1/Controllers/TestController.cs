using Microsoft.AspNetCore.Mvc;

namespace TestWebApp1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
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