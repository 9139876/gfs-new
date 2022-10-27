namespace GFS.BkgWorker;

public class BkgTaskContext
{
    private readonly Dictionary<string, object> _map = new();

    public BkgTaskContext(
        Type taskType,
        IEnumerable<KeyValuePair<string, object>>? initialCollection = null)
    {
        TaskType = taskType;
        foreach (var (key, value) in initialCollection ?? Array.Empty<KeyValuePair<string, object>>())
            _map.Add(key, value);
    }

    public string Identifire => throw new NotImplementedException();
    public Type TaskType { get; }

    public void PushOrUpdateValue(string key, object value)
    {
        if (_map.ContainsKey(key))
            _map[key] = value;
        else
            _map.Add(key, value);
    }

    public T GetValue<T>(string key)
        where T : class
    {
        if (!_map.TryGetValue(key, out var value) || value is not T tValue)
            throw new KeyNotFoundException(key);

        return tValue;
    }

    public bool TryGetValue<T>(string key, out T? tValue)
        where T : class
    {
        tValue = default;
        if (!_map.TryGetValue(key, out var value))
            return false;

        tValue = value as T;
        return tValue != default;
    }
}