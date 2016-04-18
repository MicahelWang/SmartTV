using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Formatting;

using Microsoft.Practices.Unity;
using Newtonsoft.Json;

using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.Models;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;
using YeahTVApi.Entity;
using YeahTVApi.DomainModel.Models.DataModel;
using System.Net;

namespace YeahTVApiLibrary.Filter
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CenterApiCheckSignFilterAttribute : ActionFilterAttribute
    {
        public bool GetPrivateKey { get; set; }

        public bool NeedCheckSign { get; set; }
        public bool IsCheckDeviceBind { get; set; }

        [Dependency]
        public IConstantSystemConfigManager constantSystemConfigManager { get; set; }

        [Dependency]
        public IDeviceTraceLibraryManager _DeviceTraceManager { get; set; }

        [Dependency]
        public ILogManager _LogManager { get; set; }


        [Dependency]
        public IAppLibraryManager AppManager { get; set; }


        /// <summary>
        /// 检查签名
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            string requestSign;
            string dataString;
            string deviceSeries;
            PropertyInfo property;

            string key;

            Dictionary<string, object> actionArgument = filterContext.ActionArguments;
            if (NeedCheckSign)
            {
                property = actionArgument["request"].GetType().GetProperty("Sign");
                requestSign = property.GetValue(actionArgument["request"], null).ToString();

                property = actionArgument["request"].GetType().GetProperty("Data");
                dataString = JsonConvert.SerializeObject(property.GetValue(actionArgument["request"], null));


                if (IsCheckDeviceBind)
                {
                    property = actionArgument["request"].GetType().GetProperty("DeviceSeries");
                    deviceSeries = property.GetValue(actionArgument["request"], null).ToString();
                    CheckDeviceBind(deviceSeries);
                }

                key = new StringBuilder().Append(dataString).Append(GetSignKey()).ToString().StringToMd5();

                if (string.IsNullOrWhiteSpace(requestSign) || !key.ToLower().Equals(requestSign.ToLower()))
                {
                    throw new ApplicationException("Sign Error!");
                }
            }

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {

            if (actionExecutedContext.Exception == null && actionExecutedContext.Response.StatusCode != HttpStatusCode.InternalServerError)
            {
                if (!GetPrivateKey && NeedCheckSign)
                {
                    var reponseValue = (actionExecutedContext.Response.Content as ObjectContent).Value;

                    var type = reponseValue.GetType();
                    var property = type.GetProperty("Data");
                    var returnData = property.GetValue(reponseValue, null).ToJsonString();

                    property = type.GetProperty("Sign");
                    var md5Sign = new StringBuilder().Append(returnData).Append(GetSignKey()).ToString().StringToMd5();
                    property.SetValue(reponseValue, md5Sign);
                }

                string interfaceName = string.Empty;
                if (actionExecutedContext.ActionContext.ActionArguments.ContainsKey("request"))
                {
                    interfaceName = actionExecutedContext.ActionContext.ActionArguments["request"].GetType().ToString();
                }

                _LogManager.SaveInfo(string.IsNullOrWhiteSpace(interfaceName) ? "GetPrivateKey" : interfaceName, "接口调用成功！", AppType.CommonFramework);
            }

            base.OnActionExecuted(actionExecutedContext);
        }

        private void CheckDeviceBind(string deviceSeries)
        {
            var header = new RequestHeader
            {
                DEVNO = deviceSeries,
                APP_ID = "deviceSeries"
            };

            var trace = _DeviceTraceManager.GetAppTrace(header);
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
        }

        private string GetSignKey()
        {
            return constantSystemConfigManager.StoreSignPrivateKey;
        }
    }
}
