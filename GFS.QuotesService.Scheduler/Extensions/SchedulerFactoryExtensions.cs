using Quartz;

namespace GFS.QuotesService.Scheduler.Extensions;

public static class SchedulerFactoryExtensions
{
    public static async Task<IScheduler> GetScheduler(this ISchedulerFactory factory)
    {
        var scheduler = await factory.GetScheduler("Scheduler");

        if (scheduler != null)
            return scheduler;

        await factory.GetScheduler();//Костыль для неявной первичной инициализации фабрики

        return await factory.GetScheduler("Scheduler");
    }
}