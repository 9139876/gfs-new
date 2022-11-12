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

public class CustomConfigurationActions : ICustomConfigurationActions
{
    public void ConfigureServiceCollection(IServiceCollection services, IConfiguration configuration)
    {
        services
            .RegisterDbContext<QuotesServiceDbContext>(configuration.GetConnectionString("DefaultConnection"))
            .RegisterAssemblyServicesByMember<BL.PlaceboRegistration>()
            .RegistryTinkoffRemoteApi(configuration);
    }

    public void ConfigureMapper(IServiceCollection services)
    {
        services.AddAutoMapper(expr => expr.AddProfile(new MappingProfile()), typeof(CustomConfigurationActions));
    }

    public async Task ConfigureApplication(Microsoft.AspNetCore.Builder.WebApplication application, IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();

        await serviceProvider.MigrateDatabaseAsync<QuotesServiceDbContext>();

        WorkersManager.Init(serviceProvider);
    }

    public LoggerConfiguration CustomConfigureLogger(LoggerConfiguration lc)
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