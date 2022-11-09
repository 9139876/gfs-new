using GFS.WebApplication;
using Serilog;

namespace TestWebApp1
{
    public class CustomConfigurationActions : ICustomConfigurationActions
    {
        public void ConfigureServiceCollection(IServiceCollection services, IConfiguration configuration)
        {
            
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