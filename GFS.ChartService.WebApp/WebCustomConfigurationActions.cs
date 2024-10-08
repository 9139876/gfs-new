using GFS.Api.Client.Extensions;
using GFS.ChartService.BL.Extensions;
using GFS.ChartService.BL.Mapping;
using GFS.ChartService.BL.Models.Settings;
using GFS.ChartService.DAL;
using GFS.ChartService.WebApp.Filters;
using GFS.Common.Extensions;
using GFS.EF.Extensions;
using GFS.WebApplication;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace GFS.ChartService.WebApp;

public class WebCustomConfigurationActions : WebCustomConfigurationActionsAbstract
{
    public override Action<MvcOptions> ConfigureControllers => options =>
    {
        options.Filters.Add(typeof(ErrorResultFilter));
        options.Filters.Add(typeof(ResultWrapperFilter));
    };

    public override void ConfigureServiceCollection()
    {
        ServiceCollection
            .RegisterDbContext<ChartServiceDbContext>(Configuration.GetConnectionString("DefaultConnection"))
            .RegisterRemoteApi()
            .RegisterProjectsCache()
            .RegisterAssemblyServicesByMember<BL.PlaceboRegistration>();

        ServiceCollection.Configure<ProjectStorageSettings>(Configuration.GetSection("ProjectStorageSettings"));
        ServiceCollection.Configure<SessionSettings>(Configuration.GetSection("SessionSettings"));
    }

    public override async Task ConfigureApplication()
    {
        var serviceProvider = ServiceCollection.BuildServiceProvider();
        await serviceProvider.MigrateDatabaseAsync<ChartServiceDbContext>();
    }

    public override void ConfigureMapper()
    {
        ServiceCollection.AddAutoMapper(expr => expr.AddProfile(new MappingProfile()), typeof(WebCustomConfigurationActions));
    }

    protected override LoggerConfiguration CustomConfigureLoggerInternal(LoggerConfiguration lc)
    {
        return lc
            .Enrich.WithProperty("Application", "GFS.ChartService.WebApp")
            .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
    }
}