using GFS.WebApplication.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GFS.WebApplication
{
    public static class ProgramUtilsNew
    {
        public static async Task RunWebhost<TCustomConfigurationActions>(string[] args)
            where TCustomConfigurationActions : class, ICustomConfigurationActions, new()
        {
            var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

            var customConfigurationActions = new TCustomConfigurationActions();
            customConfigurationActions.ConfigureServiceCollection(builder.Services, builder.Configuration);
            customConfigurationActions.ConfigureMapper(builder.Services);

            builder
                .ConfigureLogger(customConfigurationActions.CustomConfigureLogger)
                .Services.AddControllers()
                .AddNewtonsoftJson()
                .Services.AddEndpointsApiExplorer()
                .AddSwaggerGen();
            
            var app = builder.Build();

            app.UseSwagger()
                .UseSwaggerUI();
            
            app.MapControllers();

            await customConfigurationActions.ConfigureApplication(app, builder.Services);

            await app.RunAsync();
        }
    }
}