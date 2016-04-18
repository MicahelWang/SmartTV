namespace YeahTVApiLibrary.Filter
{
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel;
    using YeahTVApi.DomainModel.Enum;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.Entity;
    using YeahTVApiLibrary.Infrastructure;
    using Microsoft.Practices.Unity;
    using System;
    using System.Configuration;
    using System.Web;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Method)]
    public class ToolsApiFilterAttribute : ActionFilterAttribute
    {

        private const string RC4KEY = "D5BC64FAFAE95A983E56570CA70D3818";

        [Dependency]
        public ILogManager logManager { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext.Current.Items.Add("START_TIME", DateTime.Now);
            //接收到请求，
            HttpContextBase context = filterContext.HttpContext;
            //请求的数据
            ApiResultFormat fmt = WebApiCommon.GetResultFormat(filterContext);        
            RequestHeader header = null;
            String UserAgent = filterContext.HttpContext.Request.UserAgent.ToLower();

            //#region 验证
            //来自 ios 设备的请求
            if (fmt != ApiResultFormat.debug)
            {
                //处理数据
                string data = context.Request.Form[RequestParameter.Data];
                string sign = context.Request.Form[RequestParameter.Sign];
                string APP_ID = context.Request.Form[RequestParameter.APP_ID];
                byte[] buf = null;
                try
                {
                    buf = Convert.FromBase64String(data);
                }
                catch (Exception err)
                {
                    logManager.SaveError(data, err, AppType.CommonFramework, this.GetType().Name);
                    throw new ApiException("网络不给力，请重新尝试..");

                }
                var cache = System.Web.HttpRuntime.Cache;
                //data 数据解密
                SecurityManager.RC4TransformSelf(buf, System.Text.Encoding.UTF8.GetBytes(RC4KEY));
                string json_data = System.Text.Encoding.UTF8.GetString(buf);
                ////记录推送上来的数据
                buf = null;
                System.Collections.Generic.Dictionary<string, string> pams = PubFun.JsonStringToDictionary(json_data);
                //相关数据写入
                context.Items[RequestParameter.ResultKey] = pams[RequestParameter.ResultKey];
                header =pams[RequestParameter.Header].JsonStringToObj<RequestHeader>();


                //数据填充
                foreach (System.Collections.Generic.KeyValuePair<string, string> kv in pams)
                {
                    if (filterContext.ActionParameters.ContainsKey(kv.Key))
                    {
                        String value = kv.Value;
                        filterContext.ActionParameters[kv.Key] = value;

                        //HTOutputLog.LogFormat("{0}={1};\r\n", kv.Key, kv.Value);
                    }
                }
            }
            if (fmt == ApiResultFormat.debug)
            {

                foreach (var item in context.Request.QueryString.Keys)
                {
                    filterContext.ActionParameters[item.ToString()] = context.Request.QueryString[item.ToString()];
                }

                String username = "13917969197";
                String password = "123321a";
                if (ConfigurationManager.AppSettings["DebugUserName"] != null)
                {
                    username = ConfigurationManager.AppSettings["DebugUserName"].ToString();
                }
                if (ConfigurationManager.AppSettings["DebugUserPassword"] != null)
                {
                    password = ConfigurationManager.AppSettings["DebugUserPassword"].ToString();
                }

            }
            context.Items[RequestParameter.Header] = header;
            //#endregion
            foreach (var paramewter in filterContext.ActionDescriptor.GetParameters())
            {
                if (context.Items.Contains(paramewter.ParameterName))
                {
                    continue;
                }
                var attributes = paramewter.GetCustomAttributes(false);
                if (paramewter.DefaultValue != null)
                {
                    filterContext.ActionParameters[paramewter.ParameterName] = paramewter.DefaultValue;
                    context.Items[paramewter.ParameterName] = paramewter.DefaultValue;
                    continue;
                }
                
            }
            base.OnActionExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            HttpResponseBase response = filterContext.HttpContext.Response;
            if (filterContext.Result is ApiResult)
            {
                ApiResult apiResult = filterContext.Result as ApiResult;
                var ecryptionModel = new EcryptionModel();
                ecryptionModel.apiResult = apiResult;
                ecryptionModel.apiResultFormat = WebApiCommon.GetResultFormat(filterContext);
                ecryptionModel.ecryptionType = YeahTVApi.DomainModel.EcryptionType.RC4;
                ecryptionModel.PublicKey = PubFun.ConvertToString(filterContext.HttpContext.Items[RequestParameter.ResultKey]);;

                WebApiCommon.writenResponse(filterContext.HttpContext, ecryptionModel);

                //记录输出日志内容
                logManager.SaveInfo("  Response Result: ", apiResult.ToJsonString(), AppType.CommonFramework);           
            }           
            
            base.OnResultExecuted(filterContext);

            if (HttpContext.Current.Items.Contains("START_TIME"))
            {
                DateTime StartTime = (DateTime)HttpContext.Current.Items["START_TIME"];
                DateTime EndTime = DateTime.Now;
                TimeSpan span = EndTime - StartTime;
                //记录应用程序的接口访问耗时，超过5秒的才做记录
                logManager.SaveInfo("耗时:" + span.TotalSeconds, null, AppType.CommonFramework, HttpContext.Current.Request.Url.ToString());
            }              
        }
    }
}