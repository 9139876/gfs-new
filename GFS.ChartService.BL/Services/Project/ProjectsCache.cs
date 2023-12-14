using GFS.ChartService.BL.Models.ProjectModel;

namespace GFS.ChartService.BL.Services.Project;

public interface IProjectsCache
{
    void AddProject(ProjectModel projectModel);

    void UnloadProject(Guid projectId);

    bool TryGetProject(Guid projectId, out ProjectModel projectModel);
}

internal class ProjectsCache : IProjectsCache
{
    private readonly SemaphoreSlim _semaphore;

    private readonly Dictionary<Guid, ProjectModel> _cache;

    public ProjectsCache()
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

    public bool TryGetProject(Guid projectId, out ProjectModel projectModel)
    {
        try
        {
            projectModel = null;
            _semaphore.Wait();

            if (!_cache.ContainsKey(projectId))
                return false;

            projectModel = _cache[projectId];
            return true;
        }
        finally
        {
            _semaphore.Release();
        }
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
}