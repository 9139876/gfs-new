using AutoMapper;
using GFS.Broker.Api.Models.Portfolio;
using GFS.Broker.Api.Models.TestDealer;
using GFS.Broker.BL.Services;
using GFS.Broker.DAL.Entities;

namespace GFS.Broker.BL.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile(Action<IProfileExpression>? configureExt = null)
    {
        CreateMap<CreatePortfolioRequestDto, PortfolioEntity>();
        CreateMap<TryPerformPendingOrdersRequest, GetPortfolioInfoRequestDto>();
        CreateMap<PendingOrderEntity, PendingOrderDto>();
        CreateMap<PortfolioEntity, PortfolioInfoDto>()
            .ForMember(dest => dest.PortfolioId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.PortfolioState, opt => opt.MapFrom(src => src.GetPortfolioState()))
            .ForMember(dest => dest.PendingOrders, opt => opt.MapFrom((src, _, _, ctx) => ctx.Mapper.Map<PendingOrderDto>(src.PendingOrders)));

        // CreateMap<PortfolioEntity, PortfolioInfoWithHistoryDto>()
        //     .IncludeBase<PortfolioEntity, PortfolioInfoDto>()
        //     .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.DealOperations.OrderBy(o => o.MomentUtc).ToPortfolioHistory()));
        
        configureExt?.Invoke(this);
    }
}