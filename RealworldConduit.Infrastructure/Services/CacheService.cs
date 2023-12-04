using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace RealworldConduit.Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        private readonly ILogger<CacheService> _logger;
        private readonly IMemoryCache _memoryCache;

        public CacheService(ILogger<CacheService> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public T GetItem<T>(string key)
        {
            try
            {
                T item = (T)_memoryCache.Get(key);
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in GetItem for key: {key}");
                throw;
            }
        }

        public object RemoveItem(string key)
        {
            var res = true;

            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    _memoryCache.Remove(key);
                }
                else
                {
                    res = false;
                }

                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in RemoveItem for key: {key}");
                throw;
            }
        }

        public bool SetItem<T>(string key, T value, TimeSpan expirationTime)
        {
            var res = true;

            try
            {
                if (!string.IsNullOrEmpty(key) || value != null)
                {
                    _memoryCache.Set(key, value, expirationTime);
                }
                else
                {
                    res = false;
                }

                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in SetItem for key: {key}");
                throw;
            }
        }
    }
}
