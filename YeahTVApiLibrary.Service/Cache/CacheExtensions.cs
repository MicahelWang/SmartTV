using System;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTVApiLibrary.Service.Cache
{
    public static class CacheExtensions
    {
        public static T Get<T>(this IRedisCacheService cacheManager, string key, Func<T> acquire)
        {
            return Get<T>(cacheManager, key, acquire);
        }

        public static T Get<T>(this IRedisCacheService cacheManager, string key, Func<T> acquire, DateTime expiresAt)
        {
            if (cacheManager.IsSet(key))
            {
                return cacheManager.Get<T>(key);
            }
            else
            {
                var result = acquire();
                if (result == null)
                {
                    return default(T);
                }
                cacheManager.Set(key, result, expiresAt);
                return result;
            }
        }
        public static T Get<T>(this IRedisCacheService cacheManager, string key, Func<T> acquire, TimeSpan expiresIn)
        {
            if (cacheManager.IsSet(key))
            {
                return cacheManager.Get<T>(key);
            }
            else
            {
                var result = acquire();
                if (result == null)
                {
                    return default(T);
                }
                cacheManager.Set(key, result, expiresIn);
                return result;
            }
        }
    }
}