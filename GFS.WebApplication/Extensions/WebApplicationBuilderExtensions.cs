using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Events;

namespace GFS.WebApplication.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureLogger(this WebApplicationBuilder builder, Func<LoggerConfiguration, LoggerConfiguration> customConfigureLogger)
    {
        builder.Host.UseSerilog(
            (ctx, lc) =>
            {
                customConfigureLogger(lc)
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                    .WriteTo.Console()
                    .WriteTo.Debug()
                    .ReadFrom.Configuration(ctx.Configuration);
            });

        return builder;
    }
}