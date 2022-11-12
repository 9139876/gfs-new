using AutoMapper;
using GFS.Common.Extensions;
using GFS.EF.Extensions;
using GFS.QuotesService.BL.Extensions;
using GFS.QuotesService.BL.Models;
using GFS.QuotesService.DAL;
using GFS.QuotesService.DAL.Entities;
using GFS.WebApplication;
using Serilog;
using Tinkoff.InvestApi.V1;

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
        ServiceCollection.AddAutoMapper(expr => expr.AddProfile(new MappingProfile()), typeof(CustomConfigurationActions));
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

    private class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Share, InitialModel>()
                .ForMember(destination => destination.IpoDate, option => option.MapFrom(share => share.IpoDate.ToDateTime()));
            CreateMap<Currency, InitialModel>();
            CreateMap<Etf, InitialModel>();
            CreateMap<InitialModel, AssetEntity>();
            CreateMap<InitialModel, AssetInfoEntity>();
        }
    }
}