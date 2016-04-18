using System;
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
        public void Set(string key, object data)
        {
            using (var cache = RedisManager.GetClient())
            {
                cache.Set(key, data);
            }
        }
        public void Set(string key, object data, TimeSpan expiresIn)
        {
            using (var cache = RedisManager.GetClient())
            {
                cache.Set(key, data, expiresIn);
            }
        }

        public void Set(string key, object data, DateTime expiresAt)
        {
            using (var cache = RedisManager.GetClient())
            {
                cache.Set(key, data, expiresAt);
            }
        }

        public bool Add<T>(string key, T value)
        {
            var json = value.ToJsonString();
            using (var cache = RedisManager.GetClient())
            {
                return cache.Add(key, JsonConvert.DeserializeObject<T>(value.ToJsonString()));
            }
        }
        public bool Add<T>(string key, T value, DateTime expiresAt)
        {
            using (var cache = RedisManager.GetClient())
            {
                return cache.Add(key, JsonConvert.DeserializeObject<T>(value.ToJsonString()), expiresAt);
            }
        }

        public bool AddByTransaction<T>(IList<Tuple<string, T, TimeSpan>> addParameters)
        {
            using (var cache = RedisManager.GetClient())
            {
                if (addParameters == null)
                    return false;

                if (addParameters.Count == 0)
                    return false;

                addParameters.Select(a => a.Item1).ToArray();

                using (var trans = cache.CreateTransaction())
                {
                    foreach (var parameter in addParameters)
                    {

                        trans.QueueCommand(x =>
                        {
                            x.AddRangeToSet(parameter.Item1, new List<string> {parameter.Item2.ToJsonString()});
                        });
                        trans.QueueCommand(x =>
                        {
                            x.ExpireEntryIn(parameter.Item1, parameter.Item3);
                        });

                    }
                    return trans.Commit();
                }

            }
        }

        public bool AddByPipeline<T>(IList<Tuple<string, T, TimeSpan>> addParameters)
        {
            using (var cache = RedisManager.GetClient())
            {
                if (addParameters == null)
                    return false;

                if (addParameters.Count == 0)
                    return false;

                addParameters.Select(a => a.Item1).ToArray();

                try
                {
                    using (var pipeline = cache.CreatePipeline())
                    {
                        foreach (var parameter in addParameters)
                        {
                            pipeline.QueueCommand(x =>
                            {
                                cache.AddRangeToSet(parameter.Item1, new List<string> { parameter.Item2.ToJsonString() });
                                x.ExpireEntryIn(parameter.Item1, parameter.Item3);
                            });
                        }
                        pipeline.Flush();
                    }

                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }


        public bool Add<T>(string key, T value, TimeSpan expiresIn)
        {
            using (var cache = RedisManager.GetClient())
            {
                return cache.Add(key, value, expiresIn);
            }
        }

        public bool Set<T>(string key, T value)
        {
            using (var cache = RedisManager.GetClient())
            {
                return cache.Set(key, JsonConvert.DeserializeObject<T>(value.ToJsonString()));
            }
        }

        public bool Set<T>(string key, T value, DateTime expiresAt)
        {
            using (var cache = RedisManager.GetClient())
            {
                return cache.Set(key, value, expiresAt);
            }
        }

        public bool Set<T>(string key, T value, TimeSpan expiresIn)
        {
            using (var cache = RedisManager.GetClient())
            {
                return cache.Set(key, value, expiresIn);
            }
        }

        public string Get(string key)
        {
            using (var cache = RedisManager.GetClient())
            {
                return cache[key];
            }
        }

        public T Get<T>(string key)
        {
            using (var cache = RedisManager.GetClient())
            {
                return cache.Get<T>(key);
            }
        }

        [Obsolete("请使用ContainsKey方法")]
        public bool IsSet(string key)
        {
            return ContainsKey(key);
        }

        public bool ContainsKey(string key)
        {
            using (var cache = RedisManager.GetClient())
            {
                return cache.ContainsKey(key);
            }
        }

        public void Remove(string key)
        {
            using (var cache = RedisManager.GetClient())
            {
                cache.Remove(key);
            }
        }



        public void Clear()
        {
            using (var cache = RedisManager.GetClient())
            {
                cache.FlushAll();
            }
        }


        public List<string> GetAllKeys()
        {
            using (var cache = RedisManager.GetClient())
            {
                var keys = cache.GetAllKeys();
                return keys;
            }
        }
        public IDictionary<string, object> GetAllCache()
        {
            using (var cache = RedisManager.GetClient())
            {
                var keys = cache.GetAllKeys();
                return keys == null || keys.Count == 0 ? new Dictionary<string, object>() : cache.GetAll<object>(keys);
            }
        }
    }
}
