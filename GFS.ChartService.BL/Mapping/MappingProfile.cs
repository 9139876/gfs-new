using System.Drawing;
using AutoMapper;
using GFS.AnalysisSystem.Library.Calculation.Models;
using GFS.ChartService.BL.Models.ProjectViewModel.Sheet.Layers;
using GFS.ChartService.BL.Models.Responses;
using GFS.ChartService.DAL.Entities;
using GFS.GrailCommon.Models;

namespace GFS.ChartService.BL.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile(Action<IProfileExpression>? configureExt = null)
    {
        CreateMap<ProjectInfoEntity, ProjectInfoViewModel>()
            .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Name));

        //forecast
        CreateMap<Point, PriceTimePointInCells>()
            .ReverseMap();

        CreateMap<ForecastCalculationResultItem, ForecastPoint>()
            .ForMember(dest => dest.Point, opt => opt.MapFrom(src => src.Position))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Descriptions.Select(d => new ForecastItem { Description = d })));
        
        configureExt?.Invoke(this);
    }
}