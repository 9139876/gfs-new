using GFS.BackgroundWorker.Extensions;
using GFS.Common.Extensions;
using GFS.TradingStrategyTester.Api.Models;
using GFS.WebApplication;

namespace GFS.TradingStrategyTester.BackgroundWorker;

public class CustomConfigurationActions : CustomConfigurationActionsAbstract
{
    public override void ConfigureServiceCollection()
    {
        ServiceCollection
            .RegisterTaskStorage<TradingStrategyTestingItemContext>()
            .RegisterAssemblyServicesByMember<BL.PlaceboRegistration>();
    }
}