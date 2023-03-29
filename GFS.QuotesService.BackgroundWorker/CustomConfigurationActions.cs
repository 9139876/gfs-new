using GFS.BackgroundWorker.Execution;
using GFS.BackgroundWorker.Extensions;
using GFS.Common.Extensions;
using GFS.EF.Extensions;
using GFS.QuotesService.BackgroundWorker.Api.Models;
using GFS.QuotesService.BackgroundWorker.Execution;
using GFS.QuotesService.BL.Extensions;
using GFS.QuotesService.BL.Mapping;
using GFS.QuotesService.BL.Services;
using GFS.QuotesService.DAL;
using GFS.WebApplication;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace GFS.QuotesService.BackgroundWorker;

public class CustomConfigurationActions : CustomConfigurationActionsAbstract
{
    public override void ConfigureServiceCollection()
    {
        ServiceCollection
            .RegisterDbContext<QuotesServiceDbContext>(Configuration.GetConnectionString("DefaultConnection"))
            .RegisterAssemblyServicesByMember<BL.PlaceboRegistration>()
            .RegistryTinkoffRemoteApi(Configuration)
            .AddHttpClient()
            .RegisterTaskStorage<QuotesServiceBkgWorkerTaskContext>();
    }

    public override void ConfigureMapper()
    {
        ServiceCollection.AddAutoMapper(expr => expr.AddProfile(new MappingProfile()), typeof(CustomConfigurationActions));
    }

    public override async Task ConfigureApplication()
    {
        var serviceProvider = ServiceCollection.BuildServiceProvider();
        await serviceProvider.MigrateDatabaseAsync<QuotesServiceDbContext>();
    }

    public override void OnApplicationStarted()
    {
        //Грязный хак - tasksStorage получаем из rootServiceProvider, а не из scopedServiceProvider, иначе это будет синглтон из другого скоупа и у основного приложения
        //будет другой его экземпляр,а quotesProviderService и logger получаем из scopedServiceProvider, т.к. rootServiceProvider может отдавать только синглтоны, вот так вот :)
        
        var rootServiceProvider = Application.Services;
        var scopedServiceProvider = ServiceCollection.BuildServiceProvider();

        var tasksStorage = rootServiceProvider.GetRequiredService<ITasksStorage<QuotesServiceBkgWorkerTaskContext>>();

        var quotesProviderService = scopedServiceProvider.GetRequiredService<IQuotesProviderService>();
        var logger = scopedServiceProvider.GetRequiredService<ILogger>();

        WorkersManager.Init(quotesProviderService, tasksStorage, logger);
    }

    public override LoggerConfiguration CustomConfigureLogger(LoggerConfiguration lc)
    {
        return lc
            .Enrich.WithProperty("Application", "GFS.QuotesService.BackgroundWorker");
    }
}