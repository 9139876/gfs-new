using System.Collections.Concurrent;
using System.Runtime.Serialization.Json;
using Microsoft.Extensions.Caching.Memory;

namespace GFS.Configuration
{
    public interface IGFSConfiguration
    {
        Task<string?> GetValue(string key);
    }

    public class GFSConfiguration : IGFSConfiguration
    {
        private readonly IGetConfigRemoteCallService _getConfigRemoteCallService;
        
        private readonly MemoryCache _cache;
        private readonly Dictionary<string, string> _persistentConfigs;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions;

        public GFSConfiguration(IGetConfigRemoteCallService getConfigRemoteCallService)
        {   
            _cache = new MemoryCache(new MemoryCacheOptions() { SizeLimit = 1024 });
            _cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(1)//Значение размера
                // Удаляем из кэша по истечении этого времени, независимо от скользящего таймера.
                //@FixMe: Из глобальных настроек
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(10));

            //@FixMe: через рефлексию получить все неизменные конфиги из констант
            _persistentConfigs = new Dictionary<string, string>();

            _getConfigRemoteCallService = getConfigRemoteCallService;
        }

        public async Task<string?> GetValue(string key)
        {
            if (_persistentConfigs.TryGetValue(key, out var value))
                return value;

            if (_cache.TryGetValue(key, out value))
                return value;
            
            /// Метод получения данных
            value = await _getConfigRemoteCallService.Get(key);
                
            // Сохраняем данные в кэше.
            _cache.Set(key, value, _cacheEntryOptions);

            return value;
        }
    }
}