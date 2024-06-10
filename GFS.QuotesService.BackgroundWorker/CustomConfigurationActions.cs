using AutoMapper;
using GFS.Common.Extensions;
using GFS.EF.Extensions;
using GFS.QuotesService.BackgroundWorker.Workers;
using GFS.QuotesService.BL.Extensions;
using GFS.QuotesService.BL.Mapping;
using GFS.QuotesService.BL.Models;
using GFS.QuotesService.DAL;
using GFS.QuotesService.DAL.Entities;
using GFS.WebApplication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace GFS.QuotesService.BackgroundWorker;

public class CustomConfigurationActions : ConsoleCustomConfigurationActionsAbstract
{
    protected override void ConfigureServiceCollectionInternal(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection
            .RegisterDbContext<QuotesServiceDbContext>(configuration.GetConnectionString("DefaultConnection"))
            .RegisterAssemblyServicesByMember<BL.PlaceboRegistration>()
            .RegistryTinkoffRemoteApi(configuration);

        serviceCollection
            .Configure<WorkersSettings>(configuration.GetSection("WorkersSettings"));
    }

    protected override void ConfigureMapper(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddAutoMapper(expr => expr.AddProfile(new InternalMappingProfile()), typeof(CustomConfigurationActions));
        serviceCollection.AddAutoMapper(expr => expr.AddProfile(new QuotesMappingProfile()), typeof(CustomConfigurationActions));
    }

    public override async Task ConfigureApplication(IServiceProvider serviceProvider)
    {
        await serviceProvider.MigrateDatabaseAsync<QuotesServiceDbContext>();
    }

    protected override LoggerConfiguration CustomConfigureLoggerInternal(LoggerConfiguration lc)
    {
        return lc
            .Enrich.WithProperty("Application", "GFS.QuotesService.BackgroundWorker");
    }

    private class InternalMappingProfile : Profile
    {
        public InternalMappingProfile()
        {
            CreateMap<UpdateQuotesTaskEntity, UpdateQuotesTaskData>()
                .ForMember(dest => dest.EntityId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.QuotesProviderType, opt => opt.MapFrom(src => src.AssetByProvider!.QuotesProviderType))
                .ForMember(dest => dest.AssetId, opt => opt.MapFrom(src => src.AssetByProvider!.AssetId))
                .ForMember(dest => dest.TimeFrame, opt => opt.MapFrom(src => src.AssetByProvider!.TimeFrame));

            CreateMap<UpdateQuotesTaskData, GetQuotesBatchRequestModel2>();
        }
    }
}