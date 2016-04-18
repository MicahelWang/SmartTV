using System;

namespace YeahTVApiLibrary.Infrastructure
{
    public interface IRedisCacheManager
    {
        #region 过期方法
        [Obsolete("该方法已经过期。")]
        string GetCache(string key);
        [Obsolete("该方法已经过期。")]
        string GetCache(string key, byte hours);
        [Obsolete("该方法已经过期。")]
        bool SetCache(string key, string value, byte hours);
        [Obsolete("该方法已经过期。")]
        bool SetCache(string key, string value);

        #endregion

        #region New Method
        T Get<T>(string key);
        object Get(string key);

        void Set(string key, object data);
        void Set(string key, object data, TimeSpan expiresIn);

        void Set(string key, object data, DateTime expiresAt);

        bool Add<T>(string key, T value);
        bool Add<T>(string key, T value, DateTime expiresAt);
        bool Add<T>(string key, T value, TimeSpan expiresIn);
        bool Set<T>(string key, T value);
        bool Set<T>(string key, T value, DateTime expiresAt);
        bool Set<T>(string key, T value, TimeSpan expiresIn);

        bool IsSet(string key);
        void Remove(string key);
        void Clear();
        #endregion
    }
}
