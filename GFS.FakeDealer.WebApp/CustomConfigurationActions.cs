using GFS.Api.Client.Extensions;
using GFS.Common.Extensions;
using GFS.WebApplication;

namespace GFS.FakeDealer.WebApp;

public class CustomConfigurationActions : CustomConfigurationActionsAbstract
{
    public override void ConfigureServiceCollection()
    {
        ServiceCollection
            .RegisterAssemblyServicesByMember<BL.PlaceboRegistration>()
            .RegisterRemoteApi();
    }
}