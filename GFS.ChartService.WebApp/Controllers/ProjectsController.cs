using AutoMapper;
using GFS.ChartService.BL.Models.ProjectViewModel;
using GFS.ChartService.BL.Models.Requests;
using GFS.ChartService.BL.Models.Responses;
using GFS.ChartService.BL.Services;
using GFS.ChartService.BL.Services.Project;
using GFS.Common.Attributes;
using GFS.GrailCommon.Enums;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace GFS.ChartService.WebApp.Controllers;

[EnableCors("CorsPolicy")]
[Route(nameof(ProjectsController))]
public class ProjectsController : BaseControllerWithClientIdentifier
{
    private readonly IProjectService _projectService;
    private readonly ISessionService _sessionService;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProjectsController(
        IProjectService projectService,
        ISessionService sessionService,
        IMapper mapper,
        IWebHostEnvironment webHostEnvironment)
    {
        _projectService = projectService;
        _sessionService = sessionService;
        _mapper = mapper;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpPost(nameof(CreateProject))]
    public async Task<ProjectViewModel> CreateProject([FromBody] CreateProjectRequest request)
    {
        var project = await _projectService.CreateProject(request, GetClientId(), _webHostEnvironment.IsDevelopment());
        return _mapper.Map<ProjectViewModel>(project);
    }

    [HttpGet(nameof(GetExistingProjects))]
    public async Task<List<ProjectInfoViewModel>> GetExistingProjects()
    {
        return await _projectService.GetExistingProjects();
    }

    [HttpGet(nameof(GetProjectExtInfo))]
    public async Task<ProjectInfoExtViewModel> GetProjectExtInfo(Guid projectId)
    {
        var fakeResult = new ProjectInfoExtViewModel
        {
            SheetsInfos = new List<SheetInfoExtViewModel>
            {
                new("Лукойл", Description.GetDescription(TimeFrameEnum.D1), new DateTime(1999, 01, 01), new DateTime(2023, 01, 01)),
                new("Аэрофлот", Description.GetDescription(TimeFrameEnum.D1), new DateTime(1999, 01, 01), new DateTime(2023, 01, 01))
            }
        };
    
        return await Task.FromResult(fakeResult);
    }

    [HttpPost(nameof(RefreshAccess))]
    public void RefreshAccess()
    {
        _sessionService.RefreshCacheAccess(GetClientId());
    }

    [HttpPost(nameof(SaveProject))]
    public async Task SaveProject()
    {
        await _projectService.SaveProject(GetClientId());
    }

    [HttpGet(nameof(LoadProject))]
    public async Task<ProjectViewModel> LoadProject(Guid projectId)
    {
        return _mapper.Map<ProjectViewModel>(await _projectService.LoadProject(projectId, GetClientId(), _webHostEnvironment.IsDevelopment()));
    }

    [HttpPost(nameof(CloseProject))]
    public void CloseProject()
    {
        _sessionService.CloseSession(GetClientId());
    }
}