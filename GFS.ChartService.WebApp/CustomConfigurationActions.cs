using GFS.Api.Client.Extensions;
using GFS.ChartService.BL;
using GFS.Common.Extensions;
using GFS.WebApplication;

namespace GFS.ChartService.WebApp;

public class CustomConfigurationActions : CustomConfigurationActionsAbstract
{
    public override void ConfigureServiceCollection()
    {
        ServiceCollection
            .RegisterRemoteApi()
            .RegisterAssemblyServicesByMember<PlaceboRegistration>();
    }
}