using GFS.ChartService.BL.Models.Settings;
using GFS.ChartService.BL.Services.Project;
using GFS.Common.Attributes;
using GFS.Common.Extensions;
using Microsoft.Extensions.Options;

namespace GFS.ChartService.BL.Services;

public interface ISessionService
{
    void CreateSession(Guid clientId, Guid projectId, bool isDevelopment);

    void CloseSession(Guid clientId);

    void RefreshCacheAccess(Guid clientId);

    void ClearInactiveClients();

    bool TryGetProject(Guid clientId, out Guid projectId);
}

[IgnoreRegistration]
internal class SessionService : ISessionService
{
    private readonly IProjectsCache _projectsCache;
    private readonly SemaphoreSlim _semaphore;
    private readonly Dictionary<Guid, DataWithLastAccess<Guid>> _cache;
    private readonly int _maxClientInactiveAgeInSeconds;

    public SessionService(
        IOptions<SessionSettings> sessionSettings,
        IProjectsCache projectsCache)
    {
        _projectsCache = projectsCache;
        sessionSettings
            .ThrowIfNull(new InvalidOperationException("Не заданы параметры сессий"));

        _maxClientInactiveAgeInSeconds = sessionSettings.Value.MaxClientInactiveAgeInSeconds;
        _cache = new Dictionary<Guid, DataWithLastAccess<Guid>>();
        _semaphore = new SemaphoreSlim(1);
    }

    public void CreateSession(Guid clientId, Guid projectId, bool isDevelopment)
    {
        ExecuteWithSemaphore(() =>
        {
            Action onClientIdExist = isDevelopment
                ? () => _cache.Remove(clientId)
                : () => throw new InvalidOperationException($"Ошибка создания сессии - у клиента {clientId} есть незакрытый проект");

            Action onProjectIdExist = isDevelopment
                ? () => _cache.Remove(_cache.Single(x => x.Value.Data == projectId).Key)
                : throw new InvalidOperationException($"Ошибка создания сессии - проект {projectId} занят другим клиентом");

            if (_cache.ContainsKey(clientId))
                onClientIdExist.Invoke();

            if (_cache.Values.Any(v => v.Data == projectId))
                onProjectIdExist.Invoke();

            _cache.Add(clientId, new DataWithLastAccess<Guid>(projectId));
        });
    }

    public void CloseSession(Guid clientId)
    {
        ExecuteWithSemaphore(() => CloseSessionInternal(clientId));
    }

    public void RefreshCacheAccess(Guid clientId)
    {
        ExecuteWithSemaphore(() =>
        {
            if (_cache.ContainsKey(clientId))
                _cache[clientId].UpdateAccess();
        });
    }

    public void ClearInactiveClients()
    {
        ExecuteWithSemaphore(() =>
        {
            var now = DateTime.UtcNow;

            var inactive = _cache
                .Where(item => (now - item.Value.LastAccess).TotalSeconds >= _maxClientInactiveAgeInSeconds)
                .Select(item => item.Key)
                .ToList();

            inactive.ForEach(CloseSessionInternal);
        });
    }

    public bool TryGetProject(Guid clientId, out Guid projectId)
    {
        try
        {
            _semaphore.Wait();

            if (!_cache.ContainsKey(clientId))
            {
                projectId = Guid.Empty;
                return false;
            }

            _cache[clientId].UpdateAccess();
            projectId = _cache[clientId].Data;
            return true;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private void CloseSessionInternal(Guid clientId)
    {
        if (!_cache.ContainsKey(clientId))
            return;

        _projectsCache.UnloadProject(_cache[clientId].Data);
        _cache.Remove(clientId);
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

internal class DataWithLastAccess<TData>
{
    public TData Data { get; private set; }
    public DateTime LastAccess { get; private set; }

    public DataWithLastAccess(TData data)
    {
        Data = data;
        LastAccess = DateTime.UtcNow;
    }

    public void UpdateAccess()
    {
        LastAccess = DateTime.UtcNow;
    }
}