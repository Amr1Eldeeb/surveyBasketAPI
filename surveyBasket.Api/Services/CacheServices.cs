
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace surveyBasket.Api.Services
{
    public class CacheServices : ICacheServices
    {
        public IDistributedCache _distributedCache;

        public CacheServices(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
        {
          var cacheValue = await _distributedCache.GetStringAsync(key, cancellationToken);
            return cacheValue == null ? 
                null 
                :JsonSerializer.Deserialize<T>(cacheValue);
        }
        public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
        {
            await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(value),cancellationToken);
        }

        public async Task Remove(string key, CancellationToken cancellationToken = default) 
        {
           await _distributedCache.RemoveAsync(key, cancellationToken);
        }

    }
}
