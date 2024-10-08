using GFS.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using testApi1;

namespace TestWebApp1.Controllers.Api
{
    [ApiController]
    public class TestApiWithoutRequestController : TestApiWithoutRequest
    {
        public TestApiWithoutRequestController(ILogger<TestApiWithoutRequest> logger) : base(logger)
        {
        }
    
        protected override Task ExecuteInternal()
        {
            throw new NotImplementedYetException();
        }
    }
}