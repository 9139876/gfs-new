using AutoMapper;
using GFS.ChartService.BL.Models.ProjectViewModel;
using GFS.ChartService.BL.Models.ProjectViewModel.Sheet;
using GFS.ChartService.BL.Models.Requests;
using GFS.ChartService.BL.Models.Responses;
using GFS.ChartService.BL.Models.Settings;
using GFS.ChartService.DAL.Entities;
using GFS.Common.Extensions;
using GFS.EF.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GFS.ChartService.BL.Services.Project;

public interface IProjectService
{
    Task<ProjectViewModel> CreateProject(CreateProjectRequest request, Guid clientId, bool isDevelopment);

    Task<List<ProjectInfoViewModel>> GetExistingProjects();

    Task<ProjectViewModel> LoadProject(Guid projectId, Guid clientId, bool isDevelopment);

    Task SaveProject(Guid clientId);

    Task DeleteProject(Guid projectId);
}

internal class ProjectService : IProjectService
{
    private readonly ISessionService _sessionService;
    private readonly IProjectsCache _projectsCache;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly string _path;
    private const int MAX_PROJECT_NAME_LENGTH = 50;

    public ProjectService(
        ISessionService sessionService,
        IProjectsCache projectsCache,
        IDbContext dbContext,
        IMapper mapper,
        IOptions<ProjectStorageSettings> projectStorageSettings)
    {
        _sessionService = sessionService;
        _projectsCache = projectsCache;
        _dbContext = dbContext;
        _mapper = mapper;
        _path = projectStorageSettings.Value.ProjectsStoragePath;

        string.IsNullOrWhiteSpace(_path)
            .ThrowIfTrue(new InvalidOperationException("Не указан путь к хранилищу проектов"));
    }

    public async Task<ProjectViewModel> CreateProject(CreateProjectRequest request, Guid clientId, bool isDevelopment)
    {
        if (string.IsNullOrWhiteSpace(request.ProjectName))
            throw new InvalidOperationException("Имя проекта не может быть пустым");

        if (request.ProjectName.Length > MAX_PROJECT_NAME_LENGTH)
            throw new InvalidOperationException($"Имя проекта не может быть длиннее {MAX_PROJECT_NAME_LENGTH} символов");

        if (await _dbContext.GetRepository<ProjectInfoEntity>().Get(e => e.Name == request.ProjectName).AnyAsync())
            throw new InvalidOperationException($"Проект с именем '{request.ProjectName}' уже существует");

        var project = ProjectFactory.InitProject(request.ProjectName);

        _sessionService.CreateSession(clientId, project.ProjectId, isDevelopment);
        _projectsCache.AddProject(project, isDevelopment);

        return project;
    }

    public async Task<List<ProjectInfoViewModel>> GetExistingProjects()
    {
        var entities = await _dbContext.GetRepository<ProjectInfoEntity>()
            .Get()
            .ToListAsync();

        return _mapper.Map<List<ProjectInfoViewModel>>(entities);
    }

    public async Task<ProjectViewModel> LoadProject(Guid projectId, Guid clientId, bool isDevelopment)
    {
        var projectInfo = await _dbContext.GetRepository<ProjectInfoEntity>()
            .Get(p => p.Id == projectId)
            .SingleOrDefaultAsync();

        projectInfo.ThrowIfNull(new InvalidOperationException($"Проект с идентификатором {projectId} не найден"));

        var fileName = GetFileName(projectInfo!.FileGuid);

        if (!File.Exists(fileName))
            throw new InvalidOperationException($"Для проекта с идентификатором {projectId} не найден файл");

        var projectString = await File.ReadAllTextAsync(fileName);
        var projectModel = projectString.Deserialize<ProjectViewModel>() ?? throw new InvalidOperationException($"Ошибка десериализации проекта с идентификатором {projectId}");

        _sessionService.CreateSession(clientId, projectModel.ProjectId, isDevelopment);
        _projectsCache.AddProject(projectModel, isDevelopment);

        return projectModel;
    }

    public async Task SaveProject(Guid clientId)
    {
        if (!_sessionService.TryGetProject(clientId, out var projectId))
            throw new InvalidOperationException("У клиента нет активного проекта");

        if (!_projectsCache.TryGetProject(projectId, out var projectModel))
            throw new InvalidOperationException("Проект не найден в кэше");

        var projectInfo = await _dbContext.GetRepository<ProjectInfoEntity>()
            .Get(p => p.Id == projectId)
            .SingleOrDefaultAsync();

        if (projectInfo == null)
        {
            projectInfo = new ProjectInfoEntity
            {
                FileGuid = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow,
                Name = projectModel!.ProjectName!
            };

            projectInfo.SetId(projectModel.ProjectId);
            
            _dbContext.GetRepository<ProjectInfoEntity>().Insert(projectInfo);
            await _dbContext.SaveChangesAsync();
        }

        var fileName = GetFileName(projectInfo.FileGuid);

        if (File.Exists(fileName))
            File.Delete(fileName);

        await File.WriteAllTextAsync(Path.Combine(GetFileName(projectInfo.FileGuid)), projectModel!.Serialize());
    }

    public async Task DeleteProject(Guid projectId)
    {
        var projectInfo = await _dbContext.GetRepository<ProjectInfoEntity>()
            .Get(p => p.Id == projectId)
            .SingleOrDefaultAsync();

        if (projectInfo == null)
            return;

        File.Delete(GetFileName(projectInfo.FileGuid));
        _dbContext.GetRepository<ProjectInfoEntity>().Delete(projectInfo);
        await _dbContext.SaveChangesAsync();
    }

    private string GetFileName(Guid fileGuid)
        => Path.Combine(_path, $"{fileGuid}.gfsproj");
}

internal static class ProjectFactory
{
    public static ProjectViewModel InitProject(string name)
    {
        return new ProjectViewModel
        {
            ProjectId = Guid.NewGuid(),
            ProjectName = name,
            Sheets = new List<SheetViewModel>()
        };
    }
}