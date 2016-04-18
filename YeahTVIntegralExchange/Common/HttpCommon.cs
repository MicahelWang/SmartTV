using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using YeahTVIntegralExchange.Models;

namespace YeahTVIntegralExchange
{
    public class HttpCommon
    {
        #region 变量
        public CookieContainer Cookies = new CookieContainer();//Cookies
        public string UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36";//用户代理         
        public bool AllowAutoRedirect = true;//自动跳转
        public Encoding Encoding = Encoding.UTF8;//编码 
        public WebProxy Proxy = null;//代理
        public int TimeOut = 1000*120;
        public string ContentType = "application/x-www-form-urlencoded";
        #endregion
        public string HttpPost(string Url, string postDataStr)
        {

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                request.ContentType = ContentType;
                request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
                request.CookieContainer = Cookies;
                request.Timeout = TimeOut;
                request.AllowAutoRedirect = AllowAutoRedirect;
                request.UserAgent = UserAgent;
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                request.ServerCertificateValidationCallback = delegate { return true; };
                if (Cookies != null)
                {
                    request.CookieContainer = Cookies;
                }
                request.Proxy = null;
                Stream myRequestStream = request.GetRequestStream();
                StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
                myStreamWriter.Write(postDataStr);
                myStreamWriter.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (Cookies != null)
                    response.Cookies = Cookies.GetCookies(response.ResponseUri);
                Stream myResponseStream = response.GetResponseStream();
                if (response.ContentEncoding.ToLower().Contains("gzip"))
                {
                    myResponseStream = new GZipStream(myResponseStream, CompressionMode.Decompress);
                }
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                return retString;
            }
            catch (Exception err)
            {
                ResponseApiData<string> ret = new ResponseApiData<string>();
                ret.Code = (int)ApiErrorType.System;
                ret.Message = "网络异常!";
                ret.Data = "httpCommonError:" + err.Message;
                return JsonConvert.SerializeObject(ret);
            }

        }

        public string HttpGet(string Url, string postDataStr, int TimeOut = 20000)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = ContentType;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.Proxy = null;
            request.Timeout = TimeOut;
            request.AllowAutoRedirect = AllowAutoRedirect;
            request.UserAgent = UserAgent;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            if (response.ContentEncoding.ToLower().Contains("gzip"))
            {
                myResponseStream = new GZipStream(myResponseStream, CompressionMode.Decompress);
            }
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
    }
}