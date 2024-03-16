using System.Drawing;
using AutoMapper;
using GFS.AnalysisSystem.Library.Calculation.Models;
using GFS.ChartService.BL.Models.ProjectViewModel.Sheet;
using GFS.ChartService.BL.Models.ProjectViewModel.Sheet.Layers;
using GFS.ChartService.BL.Models.Responses;
using GFS.ChartService.DAL.Entities;

namespace GFS.ChartService.BL.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile(Action<IProfileExpression>? configureExt = null)
    {
        CreateMap<ProjectInfoEntity, ProjectInfoViewModel>()
            .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Name));

        //forecast
        CreateMap<Point, SheetPointInCell>()
            .ReverseMap();

        CreateMap<ForecastCalculationResultItem, ForecastPoint>()
            .ForMember(dest => dest.Point, opt => opt.MapFrom(src => src.Position))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Descriptions.Select(d => new ForecastItem { Description = d })));

        CreateMap<ForecastCalculationResult, ForecastCalculationResultViewModel>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.GetItems));
    }
}