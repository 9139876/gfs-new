using GFS.Broker.BL.Extensions;
using GFS.Broker.BL.Mapping;
using GFS.WebApplication;

namespace GFS.Broker.WebApp;

public class WebCustomConfigurationActions: WebCustomConfigurationActionsAbstract
{
    public override void ConfigureServiceCollection()
    {
        ServiceCollection
            .RegisterBlServices(Configuration);
    }
    
    public override void ConfigureMapper()
    {
        ServiceCollection.AddAutoMapper(expr => expr.AddProfile(new MappingProfile()), typeof(WebCustomConfigurationActions));
    }
}