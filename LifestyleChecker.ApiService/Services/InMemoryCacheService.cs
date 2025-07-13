
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LifestyleChecker.ApiService.Services
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public InMemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<T?> GetAsync<T>(string key)
        {
            if (_cache.TryGetValue(key, out string json))
            {
                return Task.FromResult(JsonSerializer.Deserialize<T>(json!, _jsonOptions));
            }
            return Task.FromResult<T?>(default);
        }

        public Task RemoveAsync(string key)
        {
            _cache.Remove(key);
            return Task.CompletedTask;
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var json = JsonSerializer.Serialize(value, _jsonOptions);
            var cacheEntryOptions = new MemoryCacheEntryOptions();
            if (expiry.HasValue)
            {
                cacheEntryOptions.SetAbsoluteExpiration(expiry.Value);
            }
            _cache.Set(key, json, cacheEntryOptions);
            return Task.CompletedTask;
        }
    }
}
