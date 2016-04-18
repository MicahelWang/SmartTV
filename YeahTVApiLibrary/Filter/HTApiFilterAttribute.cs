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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using YeahTVApi.DomainModel.SearchCriteria;

    [AttributeUsage(AttributeTargets.Method)]
    public class HTApiFilterAttribute : ActionFilterAttribute
    {
        public bool ShouldNotBindDevice { get; set; }

        [Dependency]
        public IAppLibraryManager AppManager { get; set; }

        [Dependency]
        public IDeviceTraceLibraryManager DeviceTraceManager { get; set; }

        [Dependency]
        public ICacheManager CacheManager { get; set; }

        [Dependency]
        public IRedisCacheManager RedisCacheManager { get; set; }

        [Dependency]
        public ILogManager LogManager { get; set; }

        [Dependency]
        public IRequestApiService RequestApiService { get; set; }

        public HTApiFilterAttribute()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext.Current.Items.Add("START_TIME", DateTime.Now);
            //接收到请求，
            var context = filterContext.HttpContext;
            //请求的数据
            var fmt = WebApiCommon.GetResultFormat(filterContext);
            RequestHeader header = null;
            var UserAgent = filterContext.HttpContext.Request.UserAgent.ToLower();
            System.Collections.Generic.Dictionary<string, string> pams = null;
            //来自 ios 设备的请求
            if (fmt != ApiResultFormat.debug)
            {
                //处理数据
                var data = context.Request.Form[RequestParameter.Data];
                var sign = context.Request.Form[RequestParameter.Sign];
                var APP_ID = context.Request.Form[RequestParameter.APP_ID];

                byte[] buf = null;
                try
                {
                    buf = Convert.FromBase64String(data);
                }
                catch (Exception err)
                {
                    LogManager.SaveError(data, err, AppType.CommonFramework, this.GetType().Name);
                    throw new ApiException("网络不给力，请重新尝试..");
                }

                var list = new Dictionary<String, Apps>();

                if (!RedisCacheManager.IsSet(Constant.AppsListKey))
                {
                    CacheManager.SetAppsList();
                    CacheManager.SetWeather();
                }

                list = RedisCacheManager.Get<Dictionary<String, Apps>>(Constant.AppsListKey);
                LogManager.SaveInfo("HTApiFilterAttribute", list.ToJsonString(), AppType.CommonFramework);

                if (!list.ContainsKey(APP_ID))
                {
                    throw new ApiException(ApiErrorType.Default, "异常使用");
                }

                var appEntity = list[APP_ID];
                if (!appEntity.Active)
                {
                    throw new ApiException(ApiErrorType.NotExistVersion, "此版本已经失效，请升级到最新版本");
                }
                context.Items[RequestParameter.APP] = appEntity;

                var decryptionModel = new DecryptionModel();
                decryptionModel.buffer = buf;
                decryptionModel.ecryptionType = EcryptionType.RC4;
                decryptionModel.PublicKey = appEntity.SecureKey;

                //data 数据解密
                if (!Constant.IsDebugModel)
                    DecryptionRequestData(decryptionModel);

                string json_data = System.Text.Encoding.UTF8.GetString(buf);
                //记录推送上来的数据
                buf = null;
                pams = PubFun.JsonStringToDictionary(json_data);
                //相关数据写入
                context.Items[RequestParameter.ResultKey] = pams[RequestParameter.ResultKey];
                header = pams[RequestParameter.Header].JsonStringToObj<RequestHeader>();

                LogManager.SaveInfo(pams.ToJsonString(), "调用接口:" + context.Request.Url.AbsoluteUri, AppType.CommonFramework);

                if (header != null)
                {
                    header.APP_ID = APP_ID;
                }

                if (!ShouldNotBindDevice)
                {
                    CheckBindDevice(context, header);
                }

                string key_sign = SignDevice(filterContext, context, header, appEntity);

                CheckUser(sign, json_data, key_sign);

                SetGuestMemberInfo(filterContext, header);
            }
            if (fmt == ApiResultFormat.debug)
            {
                pams = PubFun.JsonStringToDictionary(context.Request.Form[RequestParameter.MockData]);
                var mockHeader = context.Request.Form[RequestParameter.Header];
                header = mockHeader.JsonStringToObj<RequestHeader>();
            }

            //数据填充
            foreach (var item in pams)
            {
                if (filterContext.ActionParameters.ContainsKey(item.Key))
                {
                    var value = item.Value;
                    filterContext.ActionParameters[item.Key] = value;
                }
            }

            context.Items[RequestParameter.Header] = header;
            foreach (var parameter in filterContext.ActionDescriptor.GetParameters())
            {
                if (context.Items.Contains(parameter.ParameterName))
                {
                    continue;
                }
                //var attributes = paramewter.GetCustomAttributes(false);
                //不传并且有默认值
                if (filterContext.ActionParameters[parameter.ParameterName] == null && parameter.DefaultValue != null)
                {
                    filterContext.ActionParameters[parameter.ParameterName] = parameter.DefaultValue;
                    context.Items[parameter.ParameterName] = parameter.DefaultValue;
                    continue;
                }
                else
                {
                    if (pams.ContainsKey(parameter.ParameterName))
                    {
                        Object value = pams[parameter.ParameterName];
                        if (parameter.ParameterType.IsValueType)
                        {
                            value = Convert.ChangeType(value, parameter.ParameterType);
                        }
                        else if (parameter.ParameterType != typeof(String))
                        {
                            value = value.ToString().JsonStringToObj(parameter.ParameterType);
                        }

                        filterContext.ActionParameters[parameter.ParameterName] = value;
                        context.Items[parameter.ParameterName] = value;
                    }
                }

            }

            base.OnActionExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var response = filterContext.HttpContext.Response;

            if (filterContext.Result is ApiResult)
            {
                var apiResult = filterContext.Result as ApiResult;
                string publicKey = PubFun.ConvertToString(filterContext.HttpContext.Items[RequestParameter.ResultKey]);
                var ecryptionModel = new EcryptionModel();
                ecryptionModel.apiResult = apiResult;
                ecryptionModel.apiResultFormat = WebApiCommon.GetResultFormat(filterContext);

                if (!Constant.IsDebugModel)
                    ecryptionModel.PublicKey = publicKey;

                //加密输出内容
                EecryptionResponeData(filterContext, ecryptionModel);

                //记录输出日志内容
                LogManager.SaveInfo(
                    filterContext.RouteData.Values.Values.ToJsonString() +filterContext.HttpContext.Items[RequestParameter.Header].ToJsonString() + apiResult.ToJsonString(), 
                    "调用完成: " + filterContext.HttpContext.Request.Url.AbsoluteUri, 
                    AppType.CommonFramework);
            }

            base.OnResultExecuted(filterContext);

            if (HttpContext.Current.Items.Contains("START_TIME"))
            {
                DateTime StartTime = (DateTime)HttpContext.Current.Items["START_TIME"];
                DateTime EndTime = DateTime.Now;
                TimeSpan span = EndTime - StartTime;
                //记录应用程序的接口访问耗时，超过5秒的才做记录
                LogManager.SaveInfo("耗时:" + span.TotalSeconds, null, AppType.CommonFramework, HttpContext.Current.Request.Url.ToString());
            }
        }

        public virtual void EecryptionResponeData(ResultExecutedContext filterContext, EcryptionModel ecryptionModel)
        {
            WebApiCommon.writenResponse(filterContext.HttpContext, ecryptionModel);
        }

        public virtual void DecryptionRequestData(DecryptionModel decryptionModel)
        {
            SecurityManager.RC4TransformSelf(decryptionModel.buffer, System.Text.Encoding.UTF8.GetBytes(decryptionModel.PublicKey));
        }

        public virtual void CheckBindDevice(HttpContextBase context, RequestHeader header)
        {
            //判断该设备是否有绑定关系，如果没有绑定关系，则抛出该问题
            var trace = DeviceTraceManager.GetAppTrace(header);
            BackupDevice backupDevice = null;
            if (trace == null)
            {
                backupDevice = AppManager.GetAppBackupDevice(header);
            }
            if (trace == null && backupDevice == null)
            {
                throw new ApiException(ApiErrorType.Default, String.Format("该设备{0}尚未绑定，请绑定以后才可使用", header.DEVNO));
            }
            if (trace != null && !trace.Active)
            {
                throw new ApiException(ApiErrorType.Default, String.Format("该设备{0}已失效，无法继续使用", header.DEVNO));
            }
            if (trace != null)
            {
                header.HotelID = trace.HotelId;
                header.RoomNo = trace.RoomNo;
                context.Items[RequestParameter.TRACE] = trace;
            }
            else
            {
                header.HotelID = backupDevice.HotelId;
            }
        }

        public virtual string SignDevice(ActionExecutingContext filterContext, HttpContextBase context, RequestHeader header, Apps appEntity)
        {
            //签名检测,如果是安全数据方法, 采用该设备的key进行签名
            string key_sign = appEntity.AppKey;

            foreach (var attribute in filterContext.ActionDescriptor.GetCustomAttributes(true))
            {
                if (attribute is AppAuthorizeAttribute)
                {
                    var key = "TestAppKey"; //AppManager.GetAppKey(header);
                    if (key == null)
                    {
                        LogManager.SaveInfo("获取私钥失败" + header.APP_ID + header.DEVNO, null, AppType.CommonFramework, filterContext.ActionDescriptor.ActionName);
                        throw new ApiException(ApiErrorType.NotLogin);
                    }
                    key_sign = key;

                }

            }
            return key_sign;
        }

        public virtual void CheckUser(string sign, string json_data, string key_sign)
        {
            //判断当前的请求是否需要用户校验
            string sign_server = null;
            try
            {
                sign_server = Convert.ToBase64String(SecurityManager.GetHash(SecurityManager.HashType.MD5,
                    new StringBuilder().Append(json_data).Append(key_sign).ToString()));
                if (sign_server != sign)
                {

                    LogManager.SaveWarning("签名不匹配", AppType.CommonFramework);
                    throw new ApiException(ApiErrorType.SignError);
                }
            }
            catch (NullReferenceException err)
            {
                LogManager.SaveError(this.GetType().Name, err, AppType.CommonFramework);
                throw new ApiException("网络不给力，请重新尝试.");

            }
        }

        public virtual void SetGuestMemberInfo(ActionExecutingContext filterContext, RequestHeader header)
        {

        }
    }
}