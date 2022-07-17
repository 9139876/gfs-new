using System.Threading.Tasks;
using GFS.Common.Extensions;
using GFS.WebApplication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GFS.ATS.WebApp
{
    public class CustomConfigurationActions : ICustomConfigurationActions
    {
        public void ConfigureServiceCollection(IServiceCollection services, IConfiguration configuration)
        {
            services
                //.RegisterDbContext<PortfolioDbContext>(configuration.GetConnectionString("DefaultConnection"))
                .RegisterAssemblyServicesByMember<BL.PlaceboRegistration>();
        }

        public void ConfigureMapper(IServiceCollection services)
        {
            
        }

        public Task ConfigureApplication(Microsoft.AspNetCore.Builder.WebApplication application)
        {
            return Task.CompletedTask;
        }
    }
}