using GFS.Api.Client.Extensions;
using GFS.WebApplication;

namespace TestWebApp2
{
    public class WebCustomConfigurationActions : WebCustomConfigurationActionsAbstract
    {
        public override void ConfigureServiceCollection()
        {
            ServiceCollection.RegisterRemoteApi();
        }
    }
}