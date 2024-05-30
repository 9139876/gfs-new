using GFS.WebApplication.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using Serilog;

namespace GFS.WebApplication
{
    public static class ProgramUtils
    {
        public static async Task RunWebHost<TCustomConfigurationActions>(string[] args)
            where TCustomConfigurationActions : WebCustomConfigurationActionsAbstract, new()
        {
            var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

            var customConfigurationActions = new TCustomConfigurationActions
            {
                ServiceCollection = builder.Services,
                Configuration = builder.Configuration
            };

            customConfigurationActions.ConfigureServiceCollection();
            customConfigurationActions.ConfigureMapper();

            builder
                .ConfigureLogger(customConfigurationActions.CustomConfigureLogger, builder.Services)
                .Services.AddControllers(customConfigurationActions.ConfigureControllers)
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.Converters.Add(new StringEnumConverter()))
                .Services.AddEndpointsApiExplorer()
                .AddSwaggerGen()
                .AddSwaggerGenNewtonsoftSupport();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    configurePolicy => configurePolicy
                        .AllowAnyHeader()
                        .SetIsOriginAllowed(_ => true)
                        .AllowAnyMethod()
                        .AllowCredentials());
            });

            var app = builder.Build();

            app.UseSwagger()
                .UseSwaggerUI();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.MapControllers();

            customConfigurationActions.Application = app;
            await customConfigurationActions.ConfigureApplication();

            var lifetime = app.Lifetime;
            lifetime.ApplicationStarted.Register(customConfigurationActions.OnApplicationStarted);
            lifetime.ApplicationStopping.Register(customConfigurationActions.OnApplicationStopping);
            lifetime.ApplicationStopped.Register(customConfigurationActions.OnApplicationStopped);

            await app.RunAsync();
        }

        public static async Task RunConsoleApplication<TCustomConfigurationActions>(string[] args, Func<string[], IServiceProvider, Task> appMainTask)
            where TCustomConfigurationActions : ConsoleCustomConfigurationActionsAbstract, new()
        {
            var customConfigurationActions = new TCustomConfigurationActions();

            using var host = Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(_ => { })
                .ConfigureServices(customConfigurationActions.ConfigureServiceCollection)
                .UseSerilog((_, lc) => { customConfigurationActions.CustomConfigureLogger(lc); })
                .Build();

            await appMainTask(args, host.Services);
        }
    }
}