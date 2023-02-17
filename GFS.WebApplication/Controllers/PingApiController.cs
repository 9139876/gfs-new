using GFS.Common.Extensions;
using GFS.WebApplication.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GFS.WebApplication.Controllers;

public class PingApiController : ControllerBase
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IConfiguration _configuration;

    public PingApiController(
        IWebHostEnvironment webHostEnvironment,
        IConfiguration configuration)
    {
        _webHostEnvironment = webHostEnvironment;
        _configuration = configuration;
    }
    
    [HttpGet]
    [Route("api/ping/get")]
    public ActionResult Get()
    {
        return Ok();
    }

    [HttpGet]
    [Route("api/ping/date")]
    public DateTime GetDate()
    {
        return DateTime.Now;
    }
    
    [HttpGet]
    [Route("api/ping/get-environment")]
    public EnvResponse GetEnvironment()
    {
        return new EnvResponse
        {
            ApplicationName =_webHostEnvironment.ApplicationName,
            EnvironmentName = _webHostEnvironment.EnvironmentName,
            UserName = Environment.UserName,
            MachineName = Environment.MachineName,
            OsVersion = Environment.OSVersion.ToString()
        };
    }
    
    [HttpGet]
    [Route("api/ping/get-config")]
    public string GetConfiguration()
    {
        return _configuration.AsEnumerable().Serialize();
    }
}