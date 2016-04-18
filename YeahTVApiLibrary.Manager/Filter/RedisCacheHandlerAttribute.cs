namespace YeahTVApiLibrary.Manager
{
    using YeahTVApiLibrary.Infrastructure;
    using YeahTVApiLibrary.Manager.Handler;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;
    using System;

    public class RedisCacheHandlerAttribute : HandlerAttribute
    {
        public string Key { get; set; }

        public Type Type { get; set; }

        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            var redisCacheManager = container.Resolve<IRedisCacheManager>();
            var logManager = container.Resolve<ILogManager>();
            return new RedisCacheHandler(logManager,redisCacheManager, Key, Type);
        }
    }
}