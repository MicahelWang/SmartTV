namespace HZTVApi.Handler
{
    using HZTVApi.Infrastructure;
    using HZTVApi.Common;
    using Microsoft.Practices.Unity.InterceptionExtension;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class RedisCacheHandler : ICallHandler
    {
        private IRedisCacheManager redisCacheManager;
        private string key;
        private Type type;

        public RedisCacheHandler(IRedisCacheManager redisCacheManager, string key, Type type)
        {
            this.redisCacheManager = redisCacheManager;
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

            var jsonData = redisCacheManager.GetCache(key);
            if (!string.IsNullOrEmpty(jsonData))
            {
                var data = JsonConvert.DeserializeObject(jsonData, type);
                result = input.CreateMethodReturn(data,input.Arguments);
                return result;
            }

            result = getNext().Invoke(input, getNext);
            redisCacheManager.SetCache(key, JsonConvert.SerializeObject(result.ReturnValue));
            
            HTOutputLog.SaveInfo("方法" + input.MethodBase.Name + "执行记录： ", message + "结束执行方法: " + input.MethodBase.Name + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

            return result;
        }
    }

}