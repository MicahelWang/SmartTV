namespace HZTVApi.Filter
{
    using HZTVApi.Infrastructure;
    using HZTVApi.Handler;
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
            return new RedisCacheHandler(redisCacheManager, Key, Type);
        }
    }
}