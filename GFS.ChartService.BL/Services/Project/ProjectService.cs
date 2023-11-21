using AutoMapper;
using GFS.ChartService.BL.Models.ProjectModel;
using GFS.ChartService.BL.Models.Requests;
using GFS.ChartService.BL.Models.Responses;
using GFS.ChartService.DAL.Entities;
using GFS.EF.Repository;
using Microsoft.EntityFrameworkCore;

namespace GFS.ChartService.BL.Services.Project;

public interface IProjectService
{
    Task<ProjectModel> CreateProject(CreateProjectRequest request, Guid clientId);

    Task<List<ProjectInfoViewModel>> GetExistingProjects();
}

internal class ProjectService : IProjectService
{
    private readonly ISessionService _sessionService;
    private readonly IProjectsStorage _projectsStorage;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public ProjectService(
        ISessionService sessionService,
        IProjectsStorage projectsStorage,
        IDbContext dbContext,
        IMapper mapper)
    {
        _sessionService = sessionService;
        _projectsStorage = projectsStorage;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ProjectModel> CreateProject(CreateProjectRequest request, Guid clientId)
    {
        if (string.IsNullOrWhiteSpace(request.ProjectName))
            throw new InvalidOperationException("Имя проекта не может быть пустым");

        if (await _dbContext.GetRepository<ProjectInfoEntity>().Get(e => e.Name == request.ProjectName).AnyAsync())
            throw new InvalidOperationException($"Проект с именем '{request.ProjectName}' уже существует");

        var project = ProjectFactory.InitProject(request.ProjectName);

        _sessionService.CreateSession(clientId, project.ProjectId);
        _projectsStorage.AddProject(project);

        return project;
    }

    public async Task<List<ProjectInfoViewModel>> GetExistingProjects()
    {
        var entities = await _dbContext.GetRepository<ProjectInfoEntity>()
            .Get()
            .ToListAsync();

        return _mapper.Map<List<ProjectInfoViewModel>>(entities);
    }
}

internal static class ProjectFactory
{
    public static ProjectModel InitProject(string name)
    {
        return new ProjectModel
        {
            ProjectId = Guid.NewGuid(),
            ProjectName = name
        };
    }
}