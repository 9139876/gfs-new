using AutoMapper;
using GFS.BkgWorker.Extensions;
using GFS.Common.Extensions;
using GFS.EF.Extensions;
using GFS.GrailCommon.Models;
using GFS.QuotesService.BackgroundWorker.Api.Models;
using GFS.QuotesService.BackgroundWorker.Execution;
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
                .ForMember(dest => dest.IpoDate, opt => opt.MapFrom(src => src.IpoDate.ToDateTime()));
            CreateMap<Currency, InitialModel>();
            CreateMap<Etf, InitialModel>();
            CreateMap<InitialModel, AssetEntity>();
            CreateMap<InitialModel, AssetInfoEntity>();
            CreateMap<HistoricCandle, QuoteModel>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Time.ToDateTime()));
            CreateMap<QuoteModel, QuoteEntity>()
                .ReverseMap();

            CreateMap<BkgWorkerTaskCreateRequest, BkgWorkerTask>()
                .ReverseMap();
        }
    }
}