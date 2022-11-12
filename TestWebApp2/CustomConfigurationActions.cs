using GFS.Api.Client.Extensions;
using GFS.WebApplication;

namespace TestWebApp2
{
    public class CustomConfigurationActions : CustomConfigurationActionsAbstract
    {
        public override void ConfigureServiceCollection()
        {
            ServiceCollection.RegisterRemoteApi();
        }
    }
}