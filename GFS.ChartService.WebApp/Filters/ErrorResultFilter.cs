using System.Net;
using GFS.Common.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GFS.ChartService.WebApp.Filters;

public class ErrorResultFilter : IAsyncExceptionFilter //IAsyncResultFilter//, 
{
    private readonly ILogger<ErrorResultFilter> _logger;

    public ErrorResultFilter(ILogger<ErrorResultFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnExceptionAsync(ExceptionContext context)
    {
        var exception = context.Exception;
        _logger.LogError(exception, "Error - {error}", exception.Message);

        var response = WebAppResponseContainer.GetFailResponse(exception.Message);

        context.HttpContext.Response.ContentType = "application/json";
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        await context.HttpContext.Response.WriteAsJsonAsync(response);
    }
}