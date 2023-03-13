using GFS.Api.Client.Extensions;
using GFS.FakeDealer.BL.Extensions;
using GFS.WebApplication;

namespace GFS.FakeDealer.WebApp;

public class CustomConfigurationActions : CustomConfigurationActionsAbstract
{
    public override void ConfigureServiceCollection()
    {
        ServiceCollection
            .RegisterBlServices()
            .RegisterRemoteApi();
    }
}