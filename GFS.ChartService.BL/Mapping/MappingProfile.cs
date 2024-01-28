using AutoMapper;
using GFS.ChartService.BL.Models.Responses;
using GFS.ChartService.DAL.Entities;

namespace GFS.ChartService.BL.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile(Action<IProfileExpression>? configureExt = null)
    {
        CreateMap<ProjectInfoEntity, ProjectInfoViewModel>()
            .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Name));
    }
}