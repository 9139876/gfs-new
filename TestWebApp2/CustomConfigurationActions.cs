using System.Threading.Tasks;
using GFS.Api.Client.Extensions;
using GFS.WebApplication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using testApi1;

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

        public Task ConfigureApplication(WebApplication application)
        {
            return Task.CompletedTask;
        }
    }
}