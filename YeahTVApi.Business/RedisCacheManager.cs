namespace HZTVApi.Manager
{
    using HZTVApi.Infrastructure;

    public class RedisCacheManager : IRedisCacheManager
    {
        private IRedisCacheLocalService redisCacheLoaclService;

        public RedisCacheManager(IRedisCacheLocalService redisCacheLoaclService)
        {
            this.redisCacheLoaclService = redisCacheLoaclService;
        }

        public string GetCache(string Key)
        {
            return redisCacheLoaclService.GetCache(Key);
        }

        public string GetCache(string Key, byte Hours)
        {
            return redisCacheLoaclService.GetCache(Key, Hours);
        }

        public bool SetCache(string Key, string Value, byte Hours)
        {
            return redisCacheLoaclService.SetCache(Key, Value, Hours);
        }

        public bool SetCache(string Key, string Value)
        {
            return redisCacheLoaclService.SetCache(Key, Value);
        }
    }
}
