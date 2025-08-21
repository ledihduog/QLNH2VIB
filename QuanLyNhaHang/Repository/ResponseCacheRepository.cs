using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using QuanLyNhaHang.IRepository;
using StackExchange.Redis;

namespace QuanLyNhaHang.Repository
{
    public class ResponseCacheRepository : IResponseCacheRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public ResponseCacheRepository(IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer)
        {
            _distributedCache = distributedCache;
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task<string> GetCachResponseAsync(string cacheKey)
        {
            var cacheResponse = await _distributedCache.GetStringAsync(cacheKey);
            return string.IsNullOrEmpty(cacheResponse) ? null : cacheResponse;
        }

        public async Task SetCacheResponseAsync(string cacheKey, object response, TimeSpan timeOut)
        {
            if (response == null)
            {
                return;
            }
            var serializedResponse = JsonConvert.SerializeObject(response, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            await _distributedCache.SetStringAsync(cacheKey, serializedResponse, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeOut
            });
        }

        public async Task RemoveCacheResponseAsync(string cacheKey)
        {
            if (string.IsNullOrEmpty(cacheKey))
            {
                throw new ArgumentException("Cache key is null or empty", nameof(cacheKey));
            }
            await foreach (var key in GetKeysAsync(cacheKey + "*"))
            {
                await _distributedCache.RemoveAsync(key);
            }
        }
        private async IAsyncEnumerable<string> GetKeysAsync(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                throw new ArgumentException("Pattern is null or empty", nameof(pattern));
            }
            foreach (var endPoint in _connectionMultiplexer.GetEndPoints())
            {
                var server = _connectionMultiplexer.GetServer(endPoint);
                var keys = server.Keys(pattern: pattern);
                foreach (var key in keys)
                {
                    yield return key;
                }
            }
        }

        public async Task RemoveCache()
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            var server = redis.GetServer("localhost", 6379);
            var db = redis.GetDatabase();
            var keys = server.Keys(pattern: "*").ToList();
            foreach (var key in keys)
            {
                db.KeyDelete(key);
            }
        }
    }
}
