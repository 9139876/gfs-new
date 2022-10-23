using GFS.QuotesService.Scheduler.Helpers;
using Quartz;

namespace GFS.QuotesService.Scheduler.Extensions;

public static class ServiceCollectionQuartzConfiguratorExtensions
{
    public static void AddStoreDurablyJob<T>(this IServiceCollectionQuartzConfigurator configurator) where T : IJob
    {
        configurator.AddJob<T>(
            Keys.GetJobKey<T>(),
            cfg =>
            {
                cfg.StoreDurably(true);
            });
    }
}