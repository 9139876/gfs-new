using Quartz;

namespace GFS.QuotesService.Scheduler.Helpers;

//Переделать на атрибуты!!!
public static class Keys
{
    public static JobKey GetJobKey<TJob>() where TJob : IJob
    {
        return new JobKey($"{typeof(TJob).Name}-job", GetGroupName<TJob>());
    }

    public static TriggerKey GetTriggerKey<TJob>(object id) where TJob :  IJob
    {
        id = id != null ? $"-{id}" : string.Empty;
        return new TriggerKey($"{typeof(TJob).Name}{id}-trigger", GetGroupName<TJob>());
    }

    private static string GetGroupName<TJob>() where TJob : IJob
    {
        return typeof(TJob) switch
        {
            // Type type when type == typeof(IAuctionProceduresPickerJob) => "AuctionScheduler.Picker",
            // Type type when type == typeof(IEndAuctionProcedureJob) => "AuctionScheduler.EndAuctionProcedure",
            // Type type when type == typeof(IStartAuctionProcedureJob) => "AuctionScheduler.StartAuctionProcedure",

            _ => "UnknownGroup"
        };
    }
}