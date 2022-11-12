using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace GFS.WebApplication.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureLogger(
        this WebApplicationBuilder builder,
        Func<LoggerConfiguration, LoggerConfiguration> customConfigureLogger,
        IServiceCollection services)
    {
        builder.Host.UseSerilog(
            (ctx, lc) =>
            {
                customConfigureLogger(lc)
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                    .WriteTo.Console()
                    .WriteTo.Debug()
                    //.WriteTo.Seq("http://seq:5341")
                    .ReadFrom.Configuration(ctx.Configuration);
            });

        var serviceProvider = services.BuildServiceProvider();
        var logger = serviceProvider.GetRequiredService<ILogger<DefaultLogger>>();
        services.AddSingleton(typeof(Microsoft.Extensions.Logging.ILogger), logger);

        return builder;
    }

    private class DefaultLogger
    {
    }
}