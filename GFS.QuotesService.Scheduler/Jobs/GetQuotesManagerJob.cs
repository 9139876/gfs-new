using Quartz;

namespace GFS.QuotesService.Scheduler.Jobs;

public interface IGetQuotesManagerJob : IJob
{
}

public class GetQuotesManagerManagerJob : IGetQuotesManagerJob
{
    private readonly ISchedulerFactory _schedulerFactory;

    public GetQuotesManagerManagerJob(
        ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var scheduler = await _schedulerFactory.GetScheduler();

        
        
        // var trigger = TriggerBuilder.Create()
        //     .WithIdentity(triggerKey)
        //     .StartAt(auctionProcedure.AuctionStartDateTime.ToLocalTime())
        //     .UsingJobData(jdm)
        //     .ForJob(Keys.GetJobKey<IStartAuctionProcedureJob>())
        //     .Build();
        //
        // await scheduler.ScheduleJob(trigger);

        // подумать, где удобнее держать очередь задач - в своем queue или в шедулере 
        
        // Если не все задачи выполнены - новые триггеры, если на этот тип задач все сделано и return
        // Посмотреть в БД че надо
        // Создать джобы на каждую задачу
        // Создать триггеры для первой партии задач
        // Фсе
        throw new NotImplementedException();
    }
}