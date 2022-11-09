using GFS.Api.Client.Extensions;
using GFS.WebApplication;
using Serilog;

namespace TestWebApp2
{
    public class CustomConfigurationActions : ICustomConfigurationActions
    {
        public void ConfigureServiceCollection(IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterRemoteApi();
            //throw new System.NotImplementedException();
        }

        public void ConfigureMapper(IServiceCollection services)
        {
            //throw new System.NotImplementedException();
        }

        public Task ConfigureApplication(WebApplication application, IServiceCollection services)
        {
            return Task.CompletedTask;
        }
        
        public LoggerConfiguration CustomConfigureLogger(LoggerConfiguration lc)
        {
            return lc;
        }
    }
}