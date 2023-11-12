using AutoMapper;
using GFS.ChartService.BL.Models;
using GFS.ChartService.BL.Models.ProjectViewModel;
using GFS.ChartService.BL.Models.Requests;
using GFS.ChartService.BL.Services.Project;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace GFS.ChartService.WebApp.Controllers;

[EnableCors("CorsPolicy")]
[Route(nameof(ProjectsController))]
public class ProjectsController : BaseControllerWithClientIdentifier
{
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;

    public ProjectsController(
        IProjectService projectService,
        IMapper mapper)
    {
        _projectService = projectService;
        _mapper = mapper;
    }

    [HttpPost(nameof(CreateProject))]
    public async Task<ProjectViewModel> CreateProject([FromBody]CreateProjectRequest request)
    {
        var project = await _projectService.CreateProject(request);
        return _mapper.Map<ProjectViewModel>(project);
    }

    [HttpGet(nameof(GetExistingProjects))]
    public async Task<List<ProjectInfoViewModel>> GetExistingProjects()
    {
        return await _projectService.GetExistingProjects();
    }
}