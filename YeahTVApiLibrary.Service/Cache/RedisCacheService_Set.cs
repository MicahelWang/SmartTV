using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Newtonsoft.Json;
using YeahTVApi.Common;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Service.Cache.Redis;
using System.Linq;

namespace YeahTVApiLibrary.Service.Cache
{
    public partial class RedisCacheService : IRedisCacheService
    {
        public void AddItemToSet<T>(string setId, T item, Func<List<T>> getAllFun)
        {
            if (getAllFun != null && !ContainsKey(setId))
            {
                AddRangeToSet(setId, getAllFun());
            }
            else
            {
                using (var cache = RedisManager.GetClient())
                {
                    if (item != null)
                    {
                        cache.AddItemToSet(setId, item.ToJsonString());
                    }
                }
            }
        }

        public void AddRangeToSet<T>(string setId, List<T> items, Func<List<T>> getAllFun)
        {
            if (getAllFun != null && !ContainsKey(setId))
            {
                AddRangeToSet(setId, getAllFun());
            }
            else
            {
                AddRangeToSet(setId, items);
            }
        }


        private void AddRangeToSet<T>(string setId, List<T> items)
        {
            using (var cache = RedisManager.GetClient())
            {
                if (items == null) return;
                var bags = new ConcurrentBag<string>();
                items.AsParallel().ForAll(m => bags.Add(m.ToJsonString()));
                cache.AddRangeToSet(setId, bags.ToList());
            }
        }
        public void RemoveItemFromSet<T>(string setId, T item)
        {
            using (var cache = RedisManager.GetClient())
            {
                if (item != null)
                {
                    cache.RemoveItemFromSet(setId, item.ToJsonString());
                }
            }
        }
        public void UpdateItemFromSet<T>(string setId, T oldItem, T newItem)
        {
            using (var cache = RedisManager.GetClient())
            {
                if (!cache.ContainsKey(setId)) return;
                if (oldItem != null)
                    cache.RemoveItemFromSet(setId, oldItem.ToJsonString());
                if (newItem != null)
                    cache.AddItemToSet(setId, newItem.ToJsonString());
            }
        }
        public long GetSetCount<T>(string setId)
        {
            using (var cache = RedisManager.GetClient())
            {
                var client = cache.As<T>();
                return client.GetSetCount(client.Sets[setId]);
            }
        }
        /// <summary>
        /// 确定一个给定的值是一个集合的成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="setId"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool SetContainsItem<T>(string setId, T item)
        {
            using (var cache = RedisManager.GetClient())
            {
                var client = cache.As<T>();
                return cache.As<T>().SetContainsItem(client.Sets[setId], item);
            }
        }
        public List<T> GetAllItemsFromSet<T>(string setId)
        {
            using (var cache = RedisManager.GetClient())
            {
                return cache.As<T>().Sets[setId].ToList();
            }
        }
        public List<string> GetCacheAllItemsFromSet(string setId)
        {
            using (var cache = RedisManager.GetClient())
            {
                return cache.GetAllItemsFromSet(setId).ToList();
            }
        }
        public List<T> GetAllFromCache<T>(string setId, Func<List<T>> getAllFun)
        {
            if (ContainsKey(setId))
                return GetAllItemsFromSet<T>(setId);
            var dataList = getAllFun();
            AddRangeToSet(setId, dataList);
            return dataList;
        }
    }
}
