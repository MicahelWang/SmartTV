using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTVApiLibrary.Service
{
    public class RequestApiService : IRequestApiService
    {
        private const string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        /// <summary>  
        /// 创建HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>
        /// <param name="method">请求方法</param>
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <returns></returns>  
        public string HttpRequest(string url, string method, Dictionary<string, string> parameters,
            int? timeout)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentNullException("url");
                }
                var request = WebRequest.Create(url) as HttpWebRequest;
                request.ServicePoint.Expect100Continue = false;
                request.ServicePoint.UseNagleAlgorithm = false; //是否使用 Nagle 不使用 提高效率
                request.AllowWriteStreamBuffering = false; //数据是否缓冲 false 提高效率
                request.Method = method.ToUpper();
                request.ContentType = "application/x-www-form-urlencoded";
                request.UserAgent = DefaultUserAgent;
                request.Timeout = timeout ?? 20000;
                var requestEncoding = new UTF8Encoding();
                //如果需要POST数据  
                if (request.Method == "POST")
                {
                    request.ContentLength = 0;

                    if (!(parameters == null || parameters.Count == 0))
                    {

                        var buffer = new StringBuilder();
                        var i = 0;
                        foreach (string key in parameters.Keys)
                        {
                            buffer.AppendFormat(i > 0 ? "&{0}={1}" : "{0}={1}", key, parameters[key]);
                            i++;
                        }

                        var postData = buffer.ToString();
                        request.ContentLength = postData.Length;
                        using (var writeStream = request.GetRequestStream())
                        {

                            byte[] bytes = requestEncoding.GetBytes(postData);
                            writeStream.Write(bytes, 0, bytes.Length);
                        }
                    }
                }

                //获取响应，并设置响应编码
                var response = (HttpWebResponse)request.GetResponse();
                string encoding = response.ContentEncoding;
                if (encoding == null || encoding.Length < 1)
                {
                    encoding = "UTF-8"; //默认编码
                }
                //读取响应流
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
                string returnData = reader.ReadToEnd();
                reader.Dispose();
                response.Close();
                return returnData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Get(string url)
        {
            return HttpRequest(url, "GET", null, null);
        }

        public string Post(string url)
        {
            return HttpRequest(url, "POST", null, null);
        }

        public string HttpRequest(string url, string method)
        {
            return HttpRequest(url, method, null, null);
        }

    }
}
