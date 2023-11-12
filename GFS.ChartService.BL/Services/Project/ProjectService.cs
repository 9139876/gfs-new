using GFS.ChartService.BL.Models;
using GFS.ChartService.BL.Models.ProjectModel;
using GFS.ChartService.BL.Models.Requests;

namespace GFS.ChartService.BL.Services.Project;

public interface IProjectService
{
    Task<ProjectModel> CreateProject(CreateProjectRequest request);

    Task<List<ProjectInfoViewModel>> GetExistingProjects();
} 

internal class ProjectService : IProjectService
{
    private readonly IProjectStoreService _projectStoreService;
    private readonly IProjectsCache _projectsCache;
    
    public ProjectService(
        IProjectStoreService projectStoreService,
        IProjectsCache projectsCache)
    {
        _projectStoreService = projectStoreService;
        _projectsCache = projectsCache;
    }


    public async Task<ProjectModel> CreateProject(CreateProjectRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.ProjectName))
            throw new InvalidOperationException("Имя проекта не может быть пустым");
        
        return await _projectStoreService.CreateProject(request.ProjectName);
    }

    public async Task<List<ProjectInfoViewModel>> GetExistingProjects()
    {
        return await _projectStoreService.GetExistingProjects();
    }
}