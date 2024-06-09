using GFS.EF.Extensions;
using GFS.QuotesService.BL.Mapping;
using GFS.QuotesService.BL.Services;
using GFS.QuotesService.DAL;
using GFS.WebApplication;

namespace GFS.QuotesService.WebApp;

public class CustomConfigurationActions : WebCustomConfigurationActionsAbstract
{
    public override void ConfigureServiceCollection()
    {
        ServiceCollection
            .RegisterDbContext<QuotesServiceDbContext>(Configuration.GetConnectionString("DefaultConnection"))
            .AddScoped<IGetDataService, GetDataService>();
    }

    public override void ConfigureMapper()
    {
        ServiceCollection.AddAutoMapper(expr => expr.AddProfile(new MappingProfile()), typeof(CustomConfigurationActions));
    }
}