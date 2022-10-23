using GFS.QuotesService.Scheduler.Extensions;
using GFS.QuotesService.Scheduler.Jobs;
using GFS.WebApplication;
using Quartz;

namespace GFS.QuotesService.Scheduler;

public class CustomConfigurationActions : ICustomConfigurationActions
{
    public void ConfigureServiceCollection(IServiceCollection services, IConfiguration configuration)
    {
        //Quartz

        // services.Configure<QuartzOptions>(Configuration.GetSection("Quartz"));

        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            q.UseSimpleTypeLoader();
            q.UseInMemoryStore();

            // convert time zones using converter that can handle Windows/Linux differences
            q.UseTimeZoneConverter();

            //Add jobs
            q.AddStoreDurablyJob<IGetQuotesManagerJob>();
            
            // var interval = Math.Max(1, Configuration.GetValue<int>("Scheduler:auctionPickerIntervalInMinutes"));
            //
            // q.AddTrigger(t => t
            //     .WithIdentity(Keys.GetTriggerKey<IAuctionProceduresPickerJob>(null))
            //     .ForJob(Keys.GetJobKey<IAuctionProceduresPickerJob>())
            //     .StartAt(DateTime.UtcNow.MiddleNextMinute())
            //     .WithSimpleSchedule(x => x
            //         .WithIntervalInMinutes(interval)
            //         .RepeatForever())
            // );
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });
    }

    public void ConfigureMapper(IServiceCollection services)
    {
        
    }

    public async Task ConfigureApplication(Microsoft.AspNetCore.Builder.WebApplication application)
    {
        
    }
}