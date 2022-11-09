using GFS.WebApplication;
using Serilog;

namespace GFS.BalanceCalculation.WebApp
{
    public class CustomConfigurationActions : ICustomConfigurationActions
    {
        public void ConfigureServiceCollection(IServiceCollection services, IConfiguration configuration)
        {
            
        }

        public void ConfigureMapper(IServiceCollection services)
        {
            
        }

        public  Task ConfigureApplication(Microsoft.AspNetCore.Builder.WebApplication application, IServiceCollection services)
        {
            return Task.CompletedTask;
        }
        
        public LoggerConfiguration CustomConfigureLogger(LoggerConfiguration lc)
        {
            return lc;
        }
    }
}