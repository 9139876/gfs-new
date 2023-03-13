using AutoMapper;
using GFS.Common.Extensions;
using GFS.EF.Extensions;
using GFS.QuotesService.BackgroundWorker.Api.Models;
using GFS.QuotesService.BackgroundWorker.Execution;
using GFS.QuotesService.BL.Extensions;
using GFS.QuotesService.BL.Mapping;
using GFS.QuotesService.DAL;
using GFS.WebApplication;
using Serilog;

namespace GFS.QuotesService.BackgroundWorker;

public class CustomConfigurationActions : CustomConfigurationActionsAbstract
{
    public override void ConfigureServiceCollection()
    {
        ServiceCollection
            .RegisterDbContext<QuotesServiceDbContext>(Configuration.GetConnectionString("DefaultConnection"))
            .RegisterAssemblyServicesByMember<BL.PlaceboRegistration>()
            .RegistryTinkoffRemoteApi(Configuration);
    }

    public override void ConfigureMapper()
    {
        ServiceCollection.AddAutoMapper(expr => expr.AddProfile(new MappingProfile(AddCustomMapping)), typeof(CustomConfigurationActions));
    }

    public override async Task ConfigureApplication()
    {
        var serviceProvider = ServiceCollection.BuildServiceProvider();
        await serviceProvider.MigrateDatabaseAsync<QuotesServiceDbContext>();
    }

    public override void OnApplicationStarted()
    {
        var serviceProvider = ServiceCollection.BuildServiceProvider();
        WorkersManager.Init(serviceProvider);
    }

    public override LoggerConfiguration CustomConfigureLogger(LoggerConfiguration lc)
    {
        return lc
            .Enrich.WithProperty("Application", "GFS.QuotesService.BackgroundWorker");
    }

    private static void AddCustomMapping(IProfileExpression profile)
    {
        profile.CreateMap<BkgWorkerTaskCreateRequest, BkgWorkerTask>()
            .ReverseMap();
    }
}