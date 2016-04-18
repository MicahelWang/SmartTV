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

namespace YeahTVApiLibrary.Filter
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HCSCheckSignFilterAttribute : ActionFilterAttribute
    {
        public bool GetPrivateKey { get; set; }

        public bool NeedCheckSign { get; set; }

        [Dependency]
        public IConstantSystemConfigManager constantSystemConfigManager { get; set; }

        [Dependency]
        public IDeviceTraceLibraryManager _DeviceTraceManager { get; set; }

        [Dependency]
        public ILogManager _LogManager { get; set; }

        //[Dependency]
        //private readonly ILogManager _logManager { get; set; }

        /// <summary>
        /// 检查签名
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            string requestSign;
            string dataString;
            string serverId;
            PropertyInfo property;

            string key;

            Dictionary<string, object> actionArgument = filterContext.ActionArguments;
            if (NeedCheckSign)
            {
                property = actionArgument["request"].GetType().GetProperty("Sign");
                requestSign = property.GetValue(actionArgument["request"], null).ToString();

                property = actionArgument["request"].GetType().GetProperty("Data");
                dataString = JsonConvert.SerializeObject(property.GetValue(actionArgument["request"], null));

                property = actionArgument["request"].GetType().GetProperty("Server_Id");
                serverId = property.GetValue(actionArgument["request"], null).ToString();


                var header = new RequestHeader();
                header.DEVNO = serverId;

                key = new StringBuilder().Append(dataString)
                                .Append(SignDevice(header)).ToString().StringToMd5();

                if (string.IsNullOrWhiteSpace(requestSign) || !key.ToLower().Equals(requestSign.ToLower()))
                {
                    throw new ApplicationException("Sign Error!");
                }
            }
            else
            {
                if (GetPrivateKey)
                {
                    serverId = ((RequestHCSServerKey)(actionArgument["request"])).server_id;

                    CheckDeviceBind(serverId);      // 判断设备是否绑定如果未绑定则抛出异常
                    key = constantSystemConfigManager.HSCPublicKey;

                    property = actionArgument["request"].GetType().GetProperty("publicKey");
                    property.SetValue(actionArgument["request"], key);
                }
            }

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            PropertyInfo property;
            string serverId;

            // 取Request的值
            var requestActionArgument = actionExecutedContext.ActionContext.ActionArguments["request"];
            property = requestActionArgument.GetType().GetProperty("Server_Id", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            serverId = property.GetValue(requestActionArgument, null).ToString();

            string interfaceName;
            interfaceName = actionExecutedContext.ActionContext.ActionArguments["request"].GetType().ToString();

            if (actionExecutedContext.Exception == null)
            {
                if (!GetPrivateKey && NeedCheckSign)
                {
                    string returnData;
                    Type type;

                    var reponseValue = (actionExecutedContext.Response.Content as ObjectContent).Value;

                    // 取Response值
                    type = reponseValue.GetType();
                    property = type.GetProperty("Data");
                    returnData = property.GetValue(reponseValue, null).ToJsonString();

                    var header = new RequestHeader();
                    header.DEVNO = serverId;

                    property = type.GetProperty("Sign");
                    string md5Sign = new StringBuilder().Append(returnData).Append(SignDevice(header)).ToString().StringToMd5();
                    property.SetValue(reponseValue, md5Sign);
                }

                // 写入成功日志
                _LogManager.SaveInfo(interfaceName, "接口调用成功！", AppType.HCS, serverId);
            }

            base.OnActionExecuted(actionExecutedContext);
        }

        private void CheckDeviceBind(string serverId)
        {
            var header = new RequestHeader();
            header.DEVNO = serverId;
            header.APP_ID = "HCS";      // 建立虚拟的APP_ID

            //判断该设备是否有绑定关系，如果没有绑定关系，则抛出该问题
            var trace = _DeviceTraceManager.GetAppTrace(header);
            if (trace == null)
            {
                throw new ApiException(ApiErrorType.Default, String.Format("该设备{0}尚未绑定，请绑定以后才可使用", header.DEVNO));
            }
            if (trace != null && !trace.Active)
            {
                throw new ApiException(ApiErrorType.Default, String.Format("该设备{0}已失效，无法继续使用", header.DEVNO));
            }
        }

        private string SignDevice(RequestHeader header)
        {
            return _DeviceTraceManager.GetAppKey(header);
        }
    }
}
