using System.Threading.Tasks;
using GFS.WebApplication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        public Task ConfigureApplication(WebApplication application)
        {
            return Task.CompletedTask;
        }
    }
}