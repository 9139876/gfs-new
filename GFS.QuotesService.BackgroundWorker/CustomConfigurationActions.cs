using GFS.Common.Extensions;
using GFS.EF.Extensions;
using GFS.QuotesService.BL.Extensions;
using GFS.QuotesService.DAL;
using GFS.WebApplication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GFS.QuotesService.BackgroundWorker;

public class CustomConfigurationActions : ConsoleCustomConfigurationActionsAbstract
{
    protected override void ConfigureServiceCollectionInternal(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection
            .RegisterDbContext<QuotesServiceDbContext>(configuration.GetConnectionString("DefaultConnection"))
            .RegisterAssemblyServicesByMember<BL.PlaceboRegistration>()
            .RegistryTinkoffRemoteApi(configuration);
    }
}