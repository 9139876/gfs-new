using GFS.Broker.BL.Extensions;
using GFS.WebApplication;

namespace GFS.Broker.WebApp;

public class CustomConfigurationActions: CustomConfigurationActionsAbstract
{
    public override void ConfigureServiceCollection()
    {
        ServiceCollection
            .RegisterBlServices(Configuration);
    }
}