using AutoMapper;
using GFS.EF.Extensions;
using GFS.QuotesService.Api.Models;
using GFS.QuotesService.BL.Services;
using GFS.QuotesService.DAL;
using GFS.QuotesService.DAL.Entities;
using GFS.WebApplication;

namespace GFS.QuotesService.WebApp;

public class CustomConfigurationActions : CustomConfigurationActionsAbstract
{
    public override void ConfigureServiceCollection()
    {
        ServiceCollection
            .RegisterDbContext<QuotesServiceDbContext>(Configuration.GetConnectionString("DefaultConnection"))
            .AddScoped<IGetDataService, GetDataService>();
    }

    public override void ConfigureMapper()
    {
        ServiceCollection.AddAutoMapper(expr => expr.AddProfile(new MappingProfile()), typeof(CustomConfigurationActions));
    }

    private class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AssetEntity, AssetsInfoDto>()
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.AssetInfo != null ? src.AssetInfo.Currency : null))
                .ForMember(dest => dest.MinPriceIncrement, opt => opt.MapFrom(src => src.AssetInfo != null ? src.AssetInfo.MinPriceIncrement : null))
                .ForMember(dest => dest.Lot, opt => opt.MapFrom(src => src.AssetInfo != null ? src.AssetInfo.Lot : null))
                .ForMember(dest => dest.IpoDate, opt => opt.MapFrom(src => src.AssetInfo != null ? src.AssetInfo.IpoDate : null))
                .ForMember(dest => dest.Sector, opt => opt.MapFrom(src => src.AssetInfo != null ? src.AssetInfo.Sector : null));
        }
    }
}