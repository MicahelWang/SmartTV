using System;
using YeahTVApi.Common;

namespace YeahTVApiLibrary.Manager
{
    using YeahTVApiLibrary.Infrastructure;

    public class RedisCacheManager : IRedisCacheManager
    {


        private readonly IRedisCacheService _redisCacheLoaclService;

        public RedisCacheManager(IRedisCacheService redisCacheLoaclService)
        {
            this._redisCacheLoaclService = redisCacheLoaclService;
        }
        #region 过期方法

        [Obsolete("该方法已经过期。")]
        public string GetCache(string key)
        {
            return _redisCacheLoaclService.Get<String>(key);
        }
        [Obsolete("该方法已经过期。")]
        public string GetCache(string key, byte hours)
        {
            return _redisCacheLoaclService.Get(key); ;
        }
        [Obsolete("该方法已经过期。")]
        public bool SetCache(string key, string value, byte hours)
        {
            TimeSpan expiresIn = new TimeSpan(hours, 0, 0);
            return _redisCacheLoaclService.Set(key, value, expiresIn);
        }
        [Obsolete("该方法已经过期。")]
        public bool SetCache(string key, string value)
        {
            return _redisCacheLoaclService.Set(key, value);
        }


        #endregion 过期方法
        public T Get<T>(string key)
        {
            return _redisCacheLoaclService.Get<T>(key);
        }

        public object Get(string key)
        {
            return _redisCacheLoaclService.Get(key);
        }

        public void Set(string key, object data)
        {
            _redisCacheLoaclService.Set(key,data);
        }

        public void Set(string key, object data, TimeSpan expiresIn)
        {
            _redisCacheLoaclService.Set(key, data, expiresIn);
        }

        public void Set(string key, object data, DateTime expiresAt)
        {
            _redisCacheLoaclService.Set(key, data, expiresAt);
        }

        public bool Add<T>(string key, T value)
        {
            return _redisCacheLoaclService.Add(key, value);
        }

        public bool Add<T>(string key, T value, DateTime expiresAt)
        {
            return _redisCacheLoaclService.Add(key, value, expiresAt);
        }

        public bool Add<T>(string key, T value, TimeSpan expiresIn)
        {
            return _redisCacheLoaclService.Add(key, value, expiresIn);
        }

        public bool Set<T>(string key, T value)
        {
            return _redisCacheLoaclService.Set(key, value);
        }

        public bool Set<T>(string key, T value, DateTime expiresAt)
        {
            return _redisCacheLoaclService.Set(key, value, expiresAt);
        }

        public bool Set<T>(string key, T value, TimeSpan expiresIn)
        {
            return _redisCacheLoaclService.Set(key, value, expiresIn);
        }

        public bool IsSet(string key)
        {
            return _redisCacheLoaclService.IsSet(key);
        }

        public void Remove(string key)
        {
            _redisCacheLoaclService.Remove(key);
        }

        public void Clear()
        {
            _redisCacheLoaclService.Clear();
        }
    }
}
