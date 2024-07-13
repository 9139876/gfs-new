using GFS.Common.Extensions;
using GFS.EF.Extensions;
using GFS.QuotesService.BL.Extensions;
using GFS.QuotesService.BL.Mapping;
using GFS.QuotesService.DAL;
using GFS.WebApplication;

namespace GFS.QuotesService.WebApp;

public class CustomConfigurationActions : WebCustomConfigurationActionsAbstract
{
    public override void ConfigureServiceCollection()
    {
        ServiceCollection
            .RegisterDbContext<QuotesServiceDbContext>(Configuration.GetConnectionString("DefaultConnection"))
            .RegisterAssemblyServicesByMember<BL.PlaceboRegistration>()
            .RegistryTinkoffRemoteApi(Configuration);
    }

    public override void ConfigureMapper()
    {
        ServiceCollection.AddAutoMapper(expr => expr.AddProfile(new QuotesMappingProfile()), typeof(CustomConfigurationActions));
    }
}