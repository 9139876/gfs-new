namespace GFS.ChartService.WebApp.Middleware;

// public class ErrorResultFilter : IAsyncExceptionFilter //IAsyncResultFilter//, 
// {
//     private readonly ILogger<ErrorResultFilter> _logger;
//
//     public ErrorResultFilter(ILogger<ErrorResultFilter> logger)
//     {
//         _logger = logger;
//     }
//
//     // public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
//     // {
//     //     var result = context.Result as ObjectResult;
//     //     // var response = result?.Value as Response;
//     //     // if (response != null)
//     //     //     context.HttpContext.Response.StatusCode
//     //     //         = response.Errors.Any(x => x.Equals("SomeError")) ? 400 : 200;
//     //     
//     //     await next();
//     // }
//
//     public async Task OnExceptionAsync(ExceptionContext context)
//     {
//         var exception = context.Exception;
//         _logger.LogError(exception, "Error - {error}", exception.Message);
//
//         var response = WebAppResponseContainer<object>.GetFailResponse(exception.Message);
//         
//         context.HttpContext.Response.ContentType = "application/json";
//         context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//         await context.HttpContext.Response.WriteAsJsonAsync(response);
//     }
// }