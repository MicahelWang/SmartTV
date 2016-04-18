using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.Common;
using System.Net.Http;
using YeahTVApi.DomainModel.Enum;
namespace YeahOnlieShoppingMall.Common
{

    public class CheckSignParameter
    {
        public bool GetPrivateKey { get; set; }
        public bool NeedCheckSign { get; set; }
        public bool IsCheckDeviceBind { get; set; }
        public CheckSignParameter(IConstantSystemConfigManager _constantSystemConfigManager
            , ILogManager LogManager)
        {
            this.constantSystemConfigManager = _constantSystemConfigManager;
            this._LogManager = LogManager;
        }
        [Dependency]
        public IConstantSystemConfigManager constantSystemConfigManager { get; set; }
        [Dependency]
        public ILogManager _LogManager { get; set; }
        public bool CheckSignData(string data, string checkSign)
        {
            string key = new StringBuilder().Append(data).Append(GetSignKey()).ToString().StringToMd5();
            if (string.IsNullOrWhiteSpace(checkSign) || !key.ToLower().Equals(checkSign.ToLower()))
            {
                
                throw new ApplicationException("Sign Error!");
            }
            return true;
        }
        public string GetSignData(string data)
        {
            string key = new StringBuilder().Append(data).Append(GetSignKey()).ToString().StringToMd5();
            return key;
        }
        /// <summary>
        /// 检查签名
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuting(HttpActionContext filterContext)
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
                }

                key = new StringBuilder().Append(dataString).Append(GetSignKey()).ToString().StringToMd5();

                if (string.IsNullOrWhiteSpace(requestSign) || !key.ToLower().Equals(requestSign.ToLower()))
                {
                    throw new ApplicationException("Sign Error!");
                }
            }
        }

        public void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {

            if (actionExecutedContext.Exception == null)
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
        }


        private string GetSignKey()
        {
            return constantSystemConfigManager.StoreSignPrivateKey;
        }

    }
}