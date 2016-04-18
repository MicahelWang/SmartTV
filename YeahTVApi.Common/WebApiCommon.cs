namespace YeahTVApi.Common
{
    using YeahTVApi.DomainModel;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.Entity;
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Collections.Generic;
    using System.Linq;

    public class WebApiCommon
    {
        public static Dictionary<EcryptionType, Func<EcryptionModel, byte[]>> GetResponeBuffers =
            new Dictionary<EcryptionType, Func<EcryptionModel, byte[]>>();

        //   private static Dictionary<EcryptionModel,Func<byte[]>> GetResponeBuffers
        static WebApiCommon()
        {
            GetResponeBuffers.Add(EcryptionType.RC4, (model) =>
            {
                var keyBytes = System.Text.Encoding.UTF8.GetBytes(model.PublicKey);
                return PubFun.Obj2JsonBuffer(model.apiResult, true, keyBytes);
            });
        }

        public static ApiResultFormat GetResultFormat(ControllerContext context)
        {
            ApiResultFormat rst = ApiResultFormat.unknow;
            if (context == null) return rst;

            RouteData rt = context.RouteData;
            if (rt == null) return rst; 

            var format = context.HttpContext.Request.Form[RequestParameter.Format];
            if (!string.IsNullOrEmpty(format))
            {
                rst = format.ParseAsEnum<ApiResultFormat>();
            }
            return rst;
        }

        public static String GetPlatform(ControllerContext context)
        {
            if (context == null) return null;

            RouteData rt = context.RouteData;
            if (rt == null) return null;

            object obj = rt.Values["format"];
            if (obj == null) return null;

            return Convert.ToString(obj);

        }

        public static void writenResponse(HttpContextBase context, String result)
        {
            if (context == null) return;
            if (context.Response == null) return;
            HttpResponseBase response = context.Response;
            string str = PubFun.ConvertToString(context.Items[RequestParameter.ResultKey]);
            byte[] keyData = null;
            if (!string.IsNullOrEmpty(str))
            {
                //获取返回数据的 key
                keyData = System.Text.Encoding.UTF8.GetBytes(str);
            }
            writeJsonResponse(context, result, keyData);
            //标明数据ok
            response.AddHeader("Api-Key", "ok");

        }

        public static void writenResponse(HttpContextBase context, EcryptionModel ecryptionModel)
        {
            if (context == null) return;
            if (context.Response == null) return;
            HttpResponseBase response = context.Response;
            if (ecryptionModel.apiResultFormat == ApiResultFormat.xml)
            {
                writeXmlResponse(context, ecryptionModel);
            }
            else if (ecryptionModel.apiResultFormat == ApiResultFormat.json ||
                ecryptionModel.apiResultFormat == ApiResultFormat.debug ||
                ecryptionModel.apiResultFormat == ApiResultFormat.task)
            {
                writeJsonResponse(context, ecryptionModel);
            }
            else if (ecryptionModel.apiResultFormat == ApiResultFormat.unknow)
            {
                writeJsonResponse(context, ecryptionModel);
                //标明数据ok
                response.AddHeader("Api-Key", "ok");
            }
            else
            {
                throw new ApiException("未知的返回格式");
            }
        }

        public static void writeJsonResponse(HttpContextBase context, String result, byte[] key)
        {
            if (context == null) return;
            if (context.Response == null) return;
            HttpResponseBase response = context.Response;
            if (key == null || key.Length <= 0)
            {
                response.Write(result);
                response.ContentType = "application/json";
            }
            else
            {
                byte[] buf = YeahTVApi.Common.PubFun.Obj2JsonBuffer(result, true, key);
                writeBinaryResponse(context, buf);
            }
        }
  
        public static void writeJsonResponse(HttpContextBase context, EcryptionModel ecryptionModel)
        {
            if (context == null) return;
            if (context.Response == null) return;
            HttpResponseBase response = context.Response;
            if (string.IsNullOrEmpty(ecryptionModel.PublicKey))
            {
                string rstRst = ecryptionModel.apiResult.ToJsonString();
                response.Write(rstRst);
                response.ContentType = "application/json";
            }
            else
            {

                byte[] buf = GetResponeBuffers.SingleOrDefault(s => s.Key == ecryptionModel.ecryptionType).Value.Invoke(ecryptionModel);
                writeBinaryResponse(context, buf);
            }
        }

        public static void writeXmlResponse(HttpContextBase context, EcryptionModel ecryptionModel)
        {
            if (context == null) return;
            if (context.Response == null) return;
            HttpResponseBase response = context.Response;

            if (string.IsNullOrEmpty(ecryptionModel.PublicKey))
            {
                string rstRst = ecryptionModel.apiResult.ToJsonString();
                response.Write(rstRst);
                response.ContentType = "application/xml";
            }
            else
            {
                byte[] buf = GetResponeBuffers.SingleOrDefault(s => s.Key == ecryptionModel.ecryptionType).Value.Invoke(ecryptionModel);
                writeBinaryResponse(context, buf);
            }
        }

        public static void writeBinaryResponse(HttpContextBase context, byte[] buf)
        {
            if (context == null) return;
            if (context.Response == null) return;
            HttpResponseBase response = context.Response;

            response.BinaryWrite(buf);
            response.ContentType = "application/octet-stream";
            response.AddHeader("Content-Length", buf.Length.ToString());
        }

        public static void writenAppToolsResponse(HttpContextBase context, IApiResult result)
        {
            if (context == null) return;
            if (context.Response == null) return;
            HttpResponseBase response = context.Response;
            string str = PubFun.ConvertToString(context.Items[RequestParameter.ResultKey]);
            byte[] keyData = null;
            var ecryptionModel = new EcryptionModel();
            ecryptionModel.apiResult = result;
            ecryptionModel.apiResultFormat = ApiResultFormat.unknow;
            ecryptionModel.ecryptionType = EcryptionType.RC4;

            if (!string.IsNullOrEmpty(str))
            {
                ecryptionModel.PublicKey = str;
            }
            writeJsonResponse(context, ecryptionModel);
            //标明数据ok
            response.AddHeader("Api-Key", "ok");
        }

       
    }
}
