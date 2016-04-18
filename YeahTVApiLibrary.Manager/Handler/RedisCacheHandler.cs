namespace YeahTVApiLibrary.Manager.Handler
{
    using YeahTVApi.Common;
    using YeahTVApiLibrary.Infrastructure;
    using Microsoft.Practices.Unity.InterceptionExtension;
    using System;

    public class RedisCacheHandler : ICallHandler
    {
        private IRedisCacheManager redisCacheManager;
        private ILogManager logManager;
        private string key;
        private Type type;

        public RedisCacheHandler(ILogManager logManager, IRedisCacheManager redisCacheManager, string key, Type type)
        {
            this.redisCacheManager = redisCacheManager;
            this.logManager = logManager;
            this.type = type;
            this.key = key;
        }

        public int Order { get; set; }//这是ICallHandler的成员，表示执行顺序

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            IMethodReturn result;

            string message = "开始执行方法: " + input.MethodBase.Name + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            message = message + " 参数: ";

            for (var i = 0; i < input.Arguments.Count; i++)
            {
                message = message + string.Format(" {0}: {1}", input.Arguments.ParameterName(i), input.Arguments[i]);
            }

            var jsonData = redisCacheManager.Get(key);
            if (jsonData != null)
            {
                var data = jsonData;
                result = input.CreateMethodReturn(data,input.Arguments);
                return result;
            }

            result = getNext().Invoke(input, getNext);
            redisCacheManager.Set(key, result.ReturnValue);

            logManager.SaveInfo("方法" + input.MethodBase.Name + "执行记录： ", message + "结束执行方法: " + input.MethodBase.Name + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), YeahTVApi.DomainModel.Enum.AppType.CommonFramework);

            return result;
        }
    }

}