using System;
using System.Collections.Generic;

namespace YeahTVApiLibrary.Infrastructure
{

    public interface IRedisCacheService
    {
        T Get<T>(string key);
        string Get(string key);

        void Set(string key, object data);
        void Set(string key, object data, TimeSpan expiresIn);

        void Set(string key, object data, DateTime expiresAt);

        bool Add<T>(string key, T value);
        bool Add<T>(string key, T value, DateTime expiresAt);
        bool Add<T>(string key, T value, TimeSpan expiresIn);
        bool AddByTransaction<T>(IList<Tuple<string, T, TimeSpan>> addParameters);
        bool AddByPipeline<T>(IList<Tuple<string, T, TimeSpan>> addParameters);
        bool Set<T>(string key, T value);
        bool Set<T>(string key, T value, DateTime expiresAt);
        bool Set<T>(string key, T value, TimeSpan expiresIn);

        bool IsSet(string key);
        void Remove(string key);
        void Clear();

        List<string> GetAllKeys();
        IDictionary<string, object> GetAllCache();

        bool ContainsKey(string key);
        void AddItemToSet<T>(string setId, T item, Func<List<T>> getAllFun);
        void AddRangeToSet<T>(string setId, List<T> items, Func<List<T>> getAllFun);
        void RemoveItemFromSet<T>(string setId, T item);
        void UpdateItemFromSet<T>(string setId, T oldItem, T newItem);
        long GetSetCount<T>(string setId);

        /// <summary>
        /// 确定一个给定的值是一个集合的成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="setId"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        bool SetContainsItem<T>(string setId, T item);

        List<T> GetAllItemsFromSet<T>(string setId);
        List<T> GetAllFromCache<T>(string setId, Func<List<T>> getAllFun);
        List<string> GetCacheAllItemsFromSet(string setId);
    }
}
