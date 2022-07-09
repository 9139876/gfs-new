using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Events;
using Serilog;

namespace GFS.WebApplication.Helpers
{
    public static class Logger
    {
        public static WebApplicationBuilder ConfigureLogger(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog(
                (ctx, lc) =>
                {
                    lc
                        .MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                        .WriteTo.Console()
                        .WriteTo.Debug()
                        .ReadFrom.Configuration(ctx.Configuration);
                });

            var serviceProvider = builder.Services.BuildServiceProvider();
            var logger = serviceProvider.GetService<Microsoft.Extensions.Logging.ILogger<ApplicationLogs>>();
            builder.Services.AddSingleton(typeof(Microsoft.Extensions.Logging.ILogger), logger!);

            return builder;
        }
    }

    public class ApplicationLogs
    {
    }
}