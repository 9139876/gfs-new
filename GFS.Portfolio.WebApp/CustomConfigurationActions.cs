using System.Threading.Tasks;
using GFS.Api.Client.Extensions;
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
                .RegisterRemoteApi()
                .RegisterDbContext<PortfolioDbContext>(configuration.GetConnectionString("DefaultConnection"));
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