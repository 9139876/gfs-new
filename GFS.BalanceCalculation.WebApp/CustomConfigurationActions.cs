using GFS.WebApplication;

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

        public  Task ConfigureApplication(Microsoft.AspNetCore.Builder.WebApplication application)
        {
            return Task.CompletedTask;
        }
    }
}