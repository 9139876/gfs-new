using GFS.Common.Extensions;
using GFS.WebApplication;

namespace GFS.ATS.WebApp
{
    public class CustomConfigurationActions : CustomConfigurationActionsAbstract
    {
        public override void ConfigureServiceCollection()
        {
            ServiceCollection
                //.RegisterDbContext<PortfolioDbContext>(configuration.GetConnectionString("DefaultConnection"))
                .RegisterAssemblyServicesByMember<BL.PlaceboRegistration>();
        }
    }
}