using GFS.WebApplication.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GFS.WebApplication
{
    public static class ProgramUtils
    {
        public static async Task RunWebhost<TCustomConfigurationActions>(string[] args)
            where TCustomConfigurationActions : CustomConfigurationActionsAbstract, new()
        {
            var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

            var customConfigurationActions = new TCustomConfigurationActions()
            {
                ServiceCollection = builder.Services,
                Configuration = builder.Configuration
            };

            customConfigurationActions.ConfigureServiceCollection();
            customConfigurationActions.ConfigureMapper();

            builder
                .ConfigureLogger(customConfigurationActions.CustomConfigureLogger, builder.Services)
                .Services.AddControllers()
                .AddNewtonsoftJson()
                .Services.AddEndpointsApiExplorer()
                .AddSwaggerGen();

            var app = builder.Build();

            app.UseSwagger()
                .UseSwaggerUI();

            app.MapControllers();

            customConfigurationActions.Application = app;
            await customConfigurationActions.ConfigureApplication();

            var lifetime = app.Lifetime;
            lifetime.ApplicationStarted.Register(customConfigurationActions.OnApplicationStarted);
            lifetime.ApplicationStopping.Register(customConfigurationActions.OnApplicationStopping);
            lifetime.ApplicationStopped.Register(customConfigurationActions.OnApplicationStopped);

            await app.RunAsync();
        }
    }
}