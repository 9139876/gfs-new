using GFS.ChartService.BL.Models;
using GFS.ChartService.BL.Models.ProjectModel;
using GFS.Common.Extensions;
using Microsoft.Extensions.Options;

namespace GFS.ChartService.BL.Services.Project;

internal interface IProjectsCache
{
    void Add(Guid clientId, ProjectModel projectModel);

    void Update(Guid clientId, ProjectModel projectModel);

    bool TryGetProject(Guid clientId, out ProjectModel? projectModel);

    void RefreshCacheAccess(Guid clientId);

    void ClearInactiveClients();
}

internal class ProjectsCache : IProjectsCache
{
    private readonly Dictionary<Guid, DataWithLastAccess<ProjectModel>> _cache;
    private readonly int _maxInactiveAgeInSeconds;

    public ProjectsCache(IOptions<ProjectStorageSettings> projectStorageSettings)
    {
        projectStorageSettings
            .ThrowIfNull(new InvalidOperationException("Не заданы параметры хранилища проектов"));

        _maxInactiveAgeInSeconds = projectStorageSettings.Value.MaxInactiveAgeInSeconds;
        _cache = new Dictionary<Guid, DataWithLastAccess<ProjectModel>>();
    }

    public void Add(Guid clientId, ProjectModel projectModel)
    {
        lock (_cache)
        {
            if (_cache.ContainsKey(clientId))
                throw new InvalidOperationException($"Ошибка создания проекта - проект для клиента {clientId} уже существует");

            _cache.Add(clientId, new DataWithLastAccess<ProjectModel>(projectModel));
        }
    }

    public void Update(Guid clientId, ProjectModel projectModel)
    {
        lock (_cache)
        {
            if (!_cache.ContainsKey(clientId))
                throw new InvalidOperationException($"Ошибка обновления проекта - проект для клиента {clientId} не найден");

            _cache[clientId].Update(projectModel);
        }
    }

    public bool TryGetProject(Guid clientId, out ProjectModel? projectModel)
    {
        lock (_cache)
        {
            if (!_cache.ContainsKey(clientId))
            {
                projectModel = null;
                return false;
            }

            _cache[clientId].UpdateAccess();
            projectModel = _cache[clientId].Data;
            return true;
        }
    }

    public void RefreshCacheAccess(Guid clientId)
    {
        lock (_cache)
        {
            if (_cache.ContainsKey(clientId))
                _cache[clientId].UpdateAccess();
        }
    }

    public void ClearInactiveClients()
    {
        lock (_cache)
        {
            var now = DateTime.UtcNow;

            var inactive = _cache
                .Where(item => (now - item.Value.LastAccess).TotalSeconds >= _maxInactiveAgeInSeconds)
                .Select(item => item.Key)
                .ToList();

            inactive.ForEach(id => _cache.Remove(id));
        }
    }
}

class DataWithLastAccess<TData>
{
    public TData Data { get; private set; }
    public DateTime LastAccess { get; private set; }

    public DataWithLastAccess(TData data)
    {
        Data = data;
        LastAccess = DateTime.UtcNow;
    }

    public void Update(TData data)
    {
        Data = data;
        LastAccess = DateTime.UtcNow;
    }

    public void UpdateAccess()
    {
        LastAccess = DateTime.UtcNow;
    }
}