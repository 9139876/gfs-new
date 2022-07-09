using System.Threading.Tasks;
using GFS.WebApplication.Helpers;
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
            customConfigurationActions.ConfigureServiceCollection(builder.Services);
            customConfigurationActions.ConfigureMapper(builder.Services);

            builder
                .ConfigureLogger()
                .Services.AddControllers()
                .AddNewtonsoftJson()
                .Services.AddEndpointsApiExplorer()
                .AddSwaggerGen();

            var app = builder.Build();

            app.UseSwagger()
                .UseSwaggerUI();
            
            app.MapControllers();

            customConfigurationActions.ConfigureApplication(app);

            await app.RunAsync();
        }
    }
}