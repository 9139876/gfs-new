using System.Threading.Tasks;
using GFS.Common.Extensions;
using GFS.EF.Extensions;
using GFS.Portfolio.DAL;
using GFS.WebApplication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GFS.Portfolio.WebApp
{
    public class CustomConfigurationActions : ICustomConfigurationActions
    {
        public void ConfigureServiceCollection(IServiceCollection services, IConfiguration configuration)
        {
            services
                .RegisterDbContext<PortfolioDbContext>(configuration.GetConnectionString("DefaultConnection"))
                .RegisterAssemblyServicesByMember<BL.PlaceboRegistration>();
        }

        public void ConfigureMapper(IServiceCollection services)
        {
        }

        public async Task ConfigureApplication(Microsoft.AspNetCore.Builder.WebApplication application)
        {
            await application.Services.MigrateDatabaseAsync<PortfolioDbContext>();
        }
    }
}