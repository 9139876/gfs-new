using System.Threading.Tasks;
using GFS.WebApplication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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