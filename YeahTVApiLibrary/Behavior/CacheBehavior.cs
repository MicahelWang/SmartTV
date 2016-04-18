using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.EntityFrameworkRepository;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Service.Cache;

namespace YeahTVApiLibrary
{
    public class CacheBehavior
    {
        private static IRedisCacheService redisCacheService;

        public static IMethodReturn ReturnCacheValue(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            IMethodReturn retvalue = null;
            var returnType = ((MethodInfo)(input.MethodBase)).ReturnType;

            if (UnitOfWorkHelper.IsCacheAttributed(input.MethodBase) && Constant.ExpiresMinutes > 0 && !returnType.FullName.StartsWith("System.Void"))
            {
                var expiresMinutes = Constant.ExpiresMinutes;

                var cacheAttribute = (CacheAttribute)Attribute.GetCustomAttribute(input.MethodBase, typeof(CacheAttribute));

                if (cacheAttribute.CacheTime > 0)
                    expiresMinutes = cacheAttribute.CacheTime;

                redisCacheService = new RedisCacheService();

                var inputArguments = input.Arguments as ParameterCollection;
                var keyPrefix = input.Target.GetType().FullName + "." + input.MethodBase.Name;
                var keyPostfix = input.Arguments.ToJsonString().Replace(":", ".");
                var resultKey = keyPrefix + ":" + keyPostfix;
                var argumentsKey = keyPrefix + "_Arguments:" + keyPostfix;
                var resultList = redisCacheService.GetCacheAllItemsFromSet(resultKey);

                if (resultList.Count > 0)
                {
                    var data = resultList[0];

                    //// 反序列化时忽略属性的JsonProperty
                    //JsonSerializerSettings settings = new JsonSerializerSettings();
                    //settings.Formatting = Formatting.Indented;
                    //settings.ContractResolver = new OriginalNameContractResolver();

                    var i = 0;
                    foreach (var argument in inputArguments)
                    {
                        var convertBaseCriteria = argument as BaseSearchCriteria;

                        if (convertBaseCriteria != null && convertBaseCriteria.NeedPaging)
                        {
                            //inputArguments[i] = redisCacheService.GetCacheAllItemsFromSet(argumentsKey)[0].JsonStringToObj(argument.GetType(), settings);
                            inputArguments[i] = redisCacheService.GetCacheAllItemsFromSet(argumentsKey)[0].JsonStringToObj(argument.GetType());
                        }

                        i++;
                    }

                    //retvalue = input.CreateMethodReturn(data.JsonStringToObj(returnType, settings), inputArguments);
                    retvalue = input.CreateMethodReturn(data.JsonStringToObj(returnType), inputArguments);
                    return retvalue;
                }

                retvalue = getNext().Invoke(input, getNext);
                if (retvalue.ReturnValue == null)
                    return retvalue;

                var expiresTime = new TimeSpan(0, expiresMinutes, 0);
                var cacheList = new List<Tuple<string, object, TimeSpan>>();

                //redisCacheService.Add(resultKey, retvalue.ReturnValue, expiresTime);
                cacheList.Add(new Tuple<string, object, TimeSpan>(resultKey, retvalue.ReturnValue, expiresTime));

                foreach (var argument in input.Arguments)
                {
                    var convertBaseCriteria = argument as BaseSearchCriteria;
                    if (convertBaseCriteria != null && convertBaseCriteria.NeedPaging)
                    {
                        //redisCacheService.Add(argumentsKey, argument, expiresTime);
                        cacheList.Add(new Tuple<string, object, TimeSpan>(argumentsKey, argument, expiresTime));
                    }
                }

                redisCacheService.AddByPipeline(cacheList);
                return retvalue;
            }

            return retvalue;
        }

    }
}
