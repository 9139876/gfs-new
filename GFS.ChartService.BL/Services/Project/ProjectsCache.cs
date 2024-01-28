using GFS.ChartService.BL.Models.ProjectViewModel;

namespace GFS.ChartService.BL.Services.Project;

public interface IProjectsCache
{
    void AddProject(ProjectViewModel projectModel, bool isDevelopment);

    void UnloadProject(Guid projectId);

    bool TryGetProject(Guid projectId, out ProjectViewModel? projectModel);
}

internal class ProjectsCache : IProjectsCache
{
    private readonly SemaphoreSlim _semaphore;

    private readonly Dictionary<Guid, ProjectViewModel> _cache;

    public ProjectsCache()
    {
        _semaphore = new SemaphoreSlim(1);
        _cache = new Dictionary<Guid, ProjectViewModel>();
    }

    public void AddProject(ProjectViewModel projectModel, bool isDevelopment)
    {
        ExecuteWithSemaphore(() =>
        {
            if (_cache.ContainsKey(projectModel.ProjectId))
            {
                if (isDevelopment)
                    return;

                throw new InvalidOperationException($"Проект с идентификатором {projectModel.ProjectId} уже в загружен в оперативное хранилище");
            }

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

    public bool TryGetProject(Guid projectId, out ProjectViewModel? projectModel)
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