using GFS.ChartService.BL.Models.ProjectModel;

namespace GFS.ChartService.BL.Services.Project;

internal static class ProjectFactory
{
    public static ProjectModel InitProject(string name)
    {
        return new ProjectModel { ProjectName = name };
    }
}