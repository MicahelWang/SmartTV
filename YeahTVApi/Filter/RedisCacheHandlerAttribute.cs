namespace YeahTVApi.Filter
{
    using YeahTVApiLibrary.Infrastructure;
    using YeahTVApiLibrary.Manager.Handler;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;
    using System;

    public class RedisCacheHandlerAttribute : HandlerAttribute
    {
        public IRedisCacheManager RedisCacheManager { get; set; }

        public ILogManager logManager { get; set; }

        public string Key { get; set; }

        public Type DataType { get; set; }

        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return new RedisCacheHandler(logManager,RedisCacheManager, Key, DataType);
        }
    }
}