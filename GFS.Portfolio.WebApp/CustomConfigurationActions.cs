using GFS.Common.Extensions;
using GFS.EF.Extensions;
using GFS.Portfolio.DAL;
using GFS.WebApplication;
using Serilog;

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

        public async Task ConfigureApplication(Microsoft.AspNetCore.Builder.WebApplication application, IServiceCollection services)
        {
            await application.Services.MigrateDatabaseAsync<PortfolioDbContext>();
        }
        
        public LoggerConfiguration CustomConfigureLogger(LoggerConfiguration lc)
        {
            return lc;
        }
    }
}