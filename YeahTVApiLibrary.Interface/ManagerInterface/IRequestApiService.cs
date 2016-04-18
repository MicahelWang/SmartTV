using System.Collections.Generic;

namespace YeahTVApiLibrary.Infrastructure
{
    public interface IRequestApiService
    {
        /// <summary>  
        /// 创建的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>
        /// <param name="method">请求方法</param>
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <returns></returns>  
        string HttpRequest(string url, string method, Dictionary<string, string> parameters,
            int? timeout);

        string Get(string url);

        string Post(string url);

        string HttpRequest(string url, string method);
    }
}