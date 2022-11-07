using GFS.Common.Extensions;
using GFS.EF.Extensions;
using GFS.QuotesService.DAL;
using GFS.WebApplication;

namespace GFS.QuotesService.BackgroundWorker;

public class CustomConfigurationActions : ICustomConfigurationActions
{
    public void ConfigureServiceCollection(IServiceCollection services, IConfiguration configuration)
    {
        services
            .RegisterDbContext<QuotesServiceDbContext>(configuration.GetConnectionString("DefaultConnection"))
            .RegisterAssemblyServicesByMember<BL.PlaceboRegistration>();
        
        WorkersManager.Init(services.BuildServiceProvider());
    }

    public void ConfigureMapper(IServiceCollection services)
    {
        
    }

    public async Task ConfigureApplication(Microsoft.AspNetCore.Builder.WebApplication application)
    {
          
    }
}