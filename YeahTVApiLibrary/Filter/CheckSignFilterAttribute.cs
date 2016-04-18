using System;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.DomainModel.Models.ViewModels;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;

namespace YeahTVApiLibrary.Filter
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CheckSignFilterAttribute : ActionFilterAttribute
    {
        [Dependency]
        public IConstantSystemConfigManager constantSystemConfigManager{ get; set; }
        
        /// <summary>
        /// 检查签名
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            PaymentInfo paymentInfo = null;
            foreach (var actionArgument in filterContext.ActionArguments)
            {
                paymentInfo=actionArgument.Value as PaymentInfo;
                if (paymentInfo != null)
                {
                    break;
                }
            }

            if (paymentInfo != null)
            {
                var postData = paymentInfo.Data;
                var postSign = paymentInfo.Sign;

                var sign =
                    new StringBuilder().Append(postData)
                        .Append(constantSystemConfigManager.VodPaymentSignKey)
                        .ToString()
                        .StringToMd5();
                if (string.IsNullOrWhiteSpace(postSign) || !sign.ToLower().Equals(postSign.ToLower()))
                    throw new Exception("Sign Error!");
            }

            base.OnActionExecuting(filterContext);
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class PayMentExceptionAttribute : ExceptionFilterAttribute
    {
        [Dependency]
        public ILogManager LogManager { get; set; }

        public override void OnException(HttpActionExecutedContext filterContext)
        {
            LogManager.SaveError(
                filterContext.Exception.StackTrace,
                filterContext.Exception,
                AppType.CommonFramework,
                HttpContext.Current.Request.Url.ToString());

            filterContext.Response = new HttpResponseMessage() { Content = new StringContent(filterContext.Exception.Message) };
            base.OnException(filterContext); 
        }
    }
}