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
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace YeahTVApiLibrary.Filter
{
    [AttributeUsage(AttributeTargets.Method)]
    public class YeahApiCheckSignFilterAttribute : ActionFilterAttribute
    {
        public bool NeedCheckSign { get; set; }
        public bool IsCheckDeviceBind { get; set; }

        public bool GetPrivateKey { get; set; }

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
            string server_id;
            PropertyInfo property;

            string key;

            var header = new RequestHeader();
            Dictionary<string, object> actionArgument = filterContext.ActionArguments;

            property = actionArgument["request"].GetType().GetProperty("Server_Id");
            server_id = property.GetValue(actionArgument["request"], null).ToString();

            if (NeedCheckSign)
            {
                property = actionArgument["request"].GetType().GetProperty("Data");
                dataString = JsonConvert.SerializeObject(property.GetValue(actionArgument["request"], null));

                var stream=HttpContext.Current.Request.InputStream;
                if (stream != null && stream.Length > 0)
                {
                    stream.Position = 0;
                    var rawData = new System.IO.StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();
                    var dataArray = rawData.Substring(0, rawData.Length - 1);
                    var flag = "\"data\":";
                    var flagIndex = rawData.IndexOf(flag);
                    if (flagIndex != -1)
                        dataString = dataArray.Substring(flagIndex + flag.Length);
                }

                property = actionArgument["request"].GetType().GetProperty("Sign");
                requestSign = property.GetValue(actionArgument["request"], null).ToString();

                key = new StringBuilder().Append(dataString).Append(GetSignKey(new RequestHeader() { DEVNO = server_id })).ToString().StringToMd5();

                if (string.IsNullOrWhiteSpace(requestSign) || !key.ToLower().Equals(requestSign.ToLower()))
                {
                    throw new ApiException(ApiErrorType.SignError,"Sign Error!");
                }

                if (IsCheckDeviceBind)
                {
                    property = actionArgument["request"].GetType().GetProperty("DeviceSeries");
                    header.DEVNO = property.GetValue(actionArgument["request"], null).ToString();

                    CheckDeviceBind(header);
                }
            }
            else
            {
                if (GetPrivateKey)
                {
                    property = actionArgument["request"].GetType().GetProperty("DeviceSeries");
                    header.DEVNO = property.GetValue(actionArgument["request"], null).ToString();

                    CheckDeviceBind(header);      // 判断设备是否绑定如果未绑定则抛出异常
                    key = constantSystemConfigManager.HSCPublicKey;

                    property = actionArgument["request"].GetType().GetProperty("PublicKey");
                    property.SetValue(actionArgument["request"], key);
                }
            }

            HttpContext.Current.Items[RequestParameter.Header] = header;

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            PropertyInfo property;
            var header = new RequestHeader();

            var requestActionArgument = actionExecutedContext.ActionContext.ActionArguments["request"];
            property = requestActionArgument.GetType().GetProperty("Server_Id", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            header.DEVNO = property.GetValue(requestActionArgument, null).ToString();

            if (actionExecutedContext.Exception == null && actionExecutedContext.Response.StatusCode != HttpStatusCode.InternalServerError)
            {
                if (!GetPrivateKey && NeedCheckSign)
                {
                    var reponseValue = (actionExecutedContext.Response.Content as ObjectContent).Value;

                    var type = reponseValue.GetType();
                    property = type.GetProperty("Data");
                    var returnData = property.GetValue(reponseValue, null).ToJsonString();

                    property = type.GetProperty("Sign");
                    var md5Sign = new StringBuilder().Append(returnData).Append(GetSignKey(header)).ToString().StringToMd5();
                    property.SetValue(reponseValue, md5Sign);
                }

                string interfaceName = actionExecutedContext.ActionContext.Request.RequestUri.OriginalString;

                if (actionExecutedContext.ActionContext.ActionArguments.ContainsKey("request"))
                {
                    interfaceName +="\r\n"+actionExecutedContext.ActionContext.ActionArguments["request"].ToJsonString()+"\r\n";
                    interfaceName += ((System.Net.Http.ObjectContent)(actionExecutedContext.ActionContext.Response.Content)).Value.ToJsonString();
                }

                _LogManager.SaveInfo(string.IsNullOrWhiteSpace(interfaceName) ? "GetPrivateKey" : interfaceName, "接口调用成功！", AppType.CommonFramework);
            }

            base.OnActionExecuted(actionExecutedContext);
        }

        private void CheckDeviceBind(RequestHeader header)
        {
            header.APP_ID = "tvApi";
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
                throw new ApiException(ApiErrorType.NotExistVersion, String.Format("该设备{0}已失效，无法继续使用", header.DEVNO));
            }
            if (trace != null)
            {
                header.HotelID = trace.HotelId;
                header.RoomNo = trace.RoomNo;
            }
            if (backupDevice != null)
            {
                header.HotelID = backupDevice.HotelId;
            }

        }

        private string GetSignKey(RequestHeader header)
        {
            return _DeviceTraceManager.GetAppKey(header);
        }
    }
}
