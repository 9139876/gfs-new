using System.Net;
using GFS.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GFS.ChartService.WebApp.Filters;

public class ResultWrapperFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var response = context.Result is ObjectResult result
            ? WebAppResponseContainer.GetSuccessResponse(result.Value)
            : WebAppResponseContainer.GetSuccessResponse(null);

        context.HttpContext.Response.ContentType = "application/json";
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
        await context.HttpContext.Response.WriteAsJsonAsync(response);
    }
}