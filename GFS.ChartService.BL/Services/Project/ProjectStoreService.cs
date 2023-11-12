using AutoMapper;
using GFS.ChartService.BL.Models;
using GFS.ChartService.BL.Models.ProjectModel;
using GFS.ChartService.DAL.Entities;
using GFS.Common.Extensions;
using GFS.EF.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GFS.ChartService.BL.Services.Project;

internal interface IProjectStoreService
{
    Task<ProjectModel> CreateProject(string name);

    Task<List<ProjectInfoViewModel>> GetExistingProjects();
    // Task<Models.Project.Models.Project> LoadProject(string projectName);
    // Task SaveProject(Models.Project.Models.Project project);
}

internal class ProjectStoreService : IProjectStoreService
{
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly string _path;
    private readonly int _minimumSupportedProjectVersion;

    public ProjectStoreService(
        IOptions<ProjectStorageSettings> projectStorageSettings,
        IDbContext dbContext,
        IMapper mapper)
    {
        projectStorageSettings
            .ThrowIfNull(new InvalidOperationException("Не заданы параметры хранилища проектов"));

        _path = projectStorageSettings.Value.ProjectsStoragePath;

        string.IsNullOrWhiteSpace(_path)
            .ThrowIfTrue(new InvalidOperationException("Не указан путь к хранилищу проектов"));

        _minimumSupportedProjectVersion = projectStorageSettings.Value.MinimumSupportedProjectVersion;
        
        _dbContext = dbContext;
        _mapper = mapper;
    }

    // public List<string> GetExistingProjects()
    //     => Directory.GetFiles(_path).Where(Models.Project.Models.Project.IsSupportedVersion).Select(Models.Project.Models.Project.GetProjectName).ToList();
    //
    // public async Task<Models.Project.Models.Project> LoadProject(string projectName)
    // {
    //     var fileContent = await File.ReadAllTextAsync(Path.Combine(_path, Models.Project.Models.Project.GetFileName(projectName)));
    //     return fileContent.Deserialize<Models.Project.Models.Project>() ?? throw new InvalidOperationException($"Ошибка десериализации проекта {projectName}");
    // }
    //
    // public async Task SaveProject(Models.Project.Models.Project project)
    // {
    //     var fileContent = project.Serialize();
    //     await File.WriteAllTextAsync(Path.Combine(_path, Models.Project.Models.Project.GetFileName(project.ProjectName)), fileContent);
    // }

    public async Task<ProjectModel> CreateProject(string name)
    {
        if (await _dbContext.GetRepository<ProjectInfoEntity>().Get(e => e.Name == name).AnyAsync())
            throw new InvalidOperationException($"Проект с именем '{name}' уже существует");

        var entity = new ProjectInfoEntity
        {
            FileGuid = Guid.NewGuid(),
            CreatedDate = DateTime.UtcNow,
            ModificationDate = DateTime.UtcNow,
            ProjectVersion = _minimumSupportedProjectVersion,
            Name = name
        };

        _dbContext.GetRepository<ProjectInfoEntity>().Insert(entity);
        await _dbContext.SaveChangesAsync();

        var project = ProjectFactory.InitProject(name);
        
        var fileContent = project.Serialize();
        await File.WriteAllTextAsync(Path.Combine(_path, GetFileName(entity.FileGuid)), fileContent);

        //To cache!!!
        
        return project;
    }

    public async Task<List<ProjectInfoViewModel>> GetExistingProjects()
    {
        var entities = await _dbContext.GetRepository<ProjectInfoEntity>()
            .Get(e => e.ProjectVersion >= _minimumSupportedProjectVersion)
            .ToListAsync();

        return _mapper.Map<List<ProjectInfoViewModel>>(entities);
    }

    private static string GetFileName(Guid fileGuid)
        => $"{fileGuid}.gfsproj";
}