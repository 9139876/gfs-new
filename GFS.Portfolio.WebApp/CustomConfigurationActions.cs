using GFS.Common.Extensions;
using GFS.EF.Extensions;
using GFS.Portfolio.DAL;
using GFS.WebApplication;
using Serilog;

namespace GFS.Portfolio.WebApp
{
    public class CustomConfigurationActions : CustomConfigurationActionsAbstract
    {
        public override void ConfigureServiceCollection()
        {
            ServiceCollection
                .RegisterDbContext<PortfolioDbContext>(Configuration.GetConnectionString("DefaultConnection"))
                .RegisterAssemblyServicesByMember<BL.PlaceboRegistration>();
        }

        public override async Task ConfigureApplication()
        {
            await Application.Services.MigrateDatabaseAsync<PortfolioDbContext>();
        }
        
        public override LoggerConfiguration CustomConfigureLogger(LoggerConfiguration lc)
        {
            return lc
                .Enrich.WithProperty("Application", "GFS.Portfolio.WebApp");
        }
    }
}