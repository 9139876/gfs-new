using AutoMapper;
using GFS.GrailCommon.Models;
using GFS.QuotesService.Api.Models;
using GFS.QuotesService.BL.Models;
using GFS.QuotesService.DAL.Entities;
using Tinkoff.InvestApi.V1;

namespace GFS.QuotesService.BL.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile(Action<IProfileExpression>? configureExt = null)
    {
        CreateMap<Google.Protobuf.WellKnownTypes.Timestamp, DateTime>()
            .ConvertUsing(ts => ts.ToDateTime());

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

        CreateMap<AssetEntity, AssetsInfoDto>()
            .ForMember(dest => dest.AssetId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.AssetInfo != null ? src.AssetInfo.Currency : null))
            .ForMember(dest => dest.MinPriceIncrement, opt => opt.MapFrom(src => src.AssetInfo != null ? src.AssetInfo.MinPriceIncrement : null))
            .ForMember(dest => dest.Lot, opt => opt.MapFrom(src => src.AssetInfo != null ? src.AssetInfo.Lot : null))
            .ForMember(dest => dest.IpoDate, opt => opt.MapFrom(src => src.AssetInfo != null ? src.AssetInfo.IpoDate : null))
            .ForMember(dest => dest.Sector, opt => opt.MapFrom(src => src.AssetInfo != null ? src.AssetInfo.Sector : null));

        configureExt?.Invoke(this);
    }
}