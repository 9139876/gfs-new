using GFS.Common.Extensions;
using GFS.WebApplication;
using Serilog;

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

        public Task ConfigureApplication(Microsoft.AspNetCore.Builder.WebApplication application, IServiceCollection services)
        {
            return Task.CompletedTask;
        }

        public LoggerConfiguration CustomConfigureLogger(LoggerConfiguration lc)
        {
            return lc;
        }
    }
}