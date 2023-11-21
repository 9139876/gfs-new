using GFS.ChartService.BL.Models.ProjectModel;

namespace GFS.ChartService.BL.Services.Project;

public interface IProjectsStorage
{
    void AddProject(ProjectModel projectModel);

    // Task LoadProject(Guid projectId);
    // Task SaveProject(Guid projectId);
    void UnloadProject(Guid projectId);
    // Task DeleteProject(Guid projectId);
}

internal class ProjectsStorage : IProjectsStorage
{
    private readonly SemaphoreSlim _semaphore;
    
    private readonly Dictionary<Guid, ProjectModel> _cache;
    
    public ProjectsStorage()
    {
        _semaphore = new SemaphoreSlim(1);
        _cache = new Dictionary<Guid, ProjectModel>();
    }
    
    public void AddProject(ProjectModel projectModel)
    {
        ExecuteWithSemaphore(() =>
        {
            if (_cache.ContainsKey(projectModel.ProjectId))
                throw new InvalidOperationException($"Проект с идентификатором {projectModel.ProjectId} уже в загружен в оперативное хранилище");
            
            _cache.Add(projectModel.ProjectId, projectModel);
        });
    }

    public void UnloadProject(Guid projectId)
    {
        ExecuteWithSemaphore(() =>
        {
            if (!_cache.ContainsKey(projectId))
                return;
            
            _cache.Remove(projectId);
        });
    }

    private void ExecuteWithSemaphore(Action? action)
    {
        try
        {
            _semaphore.Wait();
            action?.Invoke();
        }
        finally
        {
            _semaphore.Release();
        }
    }
    
    private static string GetFileName(Guid fileGuid)
        => $"{fileGuid}.gfsproj";


    // public ProjectStoreService(
//         IOptions<ProjectStorageSettings> projectStorageSettings,
//         IDbContext dbContext,
//         IMapper mapper)
//     {
//         projectStorageSettings
//             .ThrowIfNull(new InvalidOperationException("Не заданы параметры хранилища проектов"));
//
//         _path = projectStorageSettings.Value.ProjectsStoragePath;
//
//         string.IsNullOrWhiteSpace(_path)
//             .ThrowIfTrue(new InvalidOperationException("Не указан путь к хранилищу проектов"));
//
//         _minimumSupportedProjectVersion = projectStorageSettings.Value.MinimumSupportedProjectVersion;
//         
//         _dbContext = dbContext;
//         _mapper = mapper;
//     }

    // var entity = new ProjectInfoEntity
    // {
    //     FileGuid = Guid.NewGuid(),
    //     CreatedDate = DateTime.UtcNow,
    //     ModificationDate = DateTime.UtcNow,
    //     Name = request.ProjectName
    // };
    //
    // _dbContext.GetRepository<ProjectInfoEntity>().Insert(entity);
    // await _dbContext.SaveChangesAsync();

    // var fileContent = project.Serialize();
    // await File.WriteAllTextAsync(Path.Combine(_path, GetFileName(entity.FileGuid)), fileContent);

    // public List<string> GetExistingProjects()
//     //     => Directory.GetFiles(_path).Where(Models.Project.Models.Project.IsSupportedVersion).Select(Models.Project.Models.Project.GetProjectName).ToList();
//     //
//     // public async Task<Models.Project.Models.Project> LoadProject(string projectName)
//     // {
//     //     var fileContent = await File.ReadAllTextAsync(Path.Combine(_path, Models.Project.Models.Project.GetFileName(projectName)));
//     //     return fileContent.Deserialize<Models.Project.Models.Project>() ?? throw new InvalidOperationException($"Ошибка десериализации проекта {projectName}");
//     // }
//     //
//     // public async Task SaveProject(Models.Project.Models.Project project)
//     // {
//     //     var fileContent = project.Serialize();
//     //     await File.WriteAllTextAsync(Path.Combine(_path, Models.Project.Models.Project.GetFileName(project.ProjectName)), fileContent);
//     // }
}