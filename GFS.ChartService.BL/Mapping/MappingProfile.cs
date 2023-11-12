using AutoMapper;
using GFS.ChartService.BL.Models;
using GFS.ChartService.BL.Models.ProjectModel;
using GFS.ChartService.BL.Models.ProjectViewModel;
using GFS.ChartService.DAL.Entities;

namespace GFS.ChartService.BL.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile(Action<IProfileExpression>? configureExt = null)
    {
        CreateMap<ProjectInfoEntity, ProjectInfoViewModel>()
            .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Name));

        CreateMap<ProjectModel, ProjectViewModel>();
    }
}