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
    [AttributeUsage(AttributeTargets.All)]
    public class TvApiActionFilterAttribute : ActionFilterAttribute
    {
        [Dependency]
        public ILogManager LogManager { get; set; }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var url = actionExecutedContext.ActionContext.Request.RequestUri.OriginalString;

            string responseInfo = string.Empty; ;
            if (actionExecutedContext.ActionContext.Response != null)
            {
                responseInfo =
                    ((System.Net.Http.ObjectContent) (actionExecutedContext.ActionContext.Response.Content)).Value
                        .ToJsonString();
            }
            else if (actionExecutedContext.Exception != null)
            {
                responseInfo = actionExecutedContext.Exception.ToJsonString();
            }

            var requestInfo = actionExecutedContext.ActionContext.ActionArguments.ToJsonString();

            LogManager.SaveInfo(string.Format("RequestInfo：{0} @@@ ResponseInfo：{1}", requestInfo, responseInfo),
                "接口调用成功：" + url, AppType.CommonFramework);

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
