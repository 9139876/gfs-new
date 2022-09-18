using GFS.Common.Extensions;
using GFS.WebApplication;

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