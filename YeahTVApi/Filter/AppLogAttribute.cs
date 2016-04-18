namespace YeahTVApi.Filter
{
    using YeahTVApi.Common;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using YeahTVApi.Entity;
    using Microsoft.Practices.Unity;
    using YeahTVApiLibrary.Infrastructure;
    using YeahTVApi.DomainModel.Enum;

    public class AppLogAttribute : FilterAttribute, IActionFilter
    {
        [Dependency]
        public ILogManager logManager { get; set; }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var result = filterContext.Result as ApiResult != null ? (filterContext.Result as ApiResult).Data : null ;

            if (result == null)
                result = filterContext.Result as JsonResult != null ? (filterContext.Result as JsonResult).Data : null ;

            var log = "Action " + filterContext.ActionDescriptor.ActionName + "执行结果： ";

            logManager.SaveInfo(log + JsonConvert.SerializeObject(result), null, AppType.TV, filterContext.ActionDescriptor.ActionName);
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var log = "Action " + filterContext.ActionDescriptor.ActionName + "执行记录： ";
            
            foreach(var parameter in filterContext.ActionParameters)
            {
                log += "参数：" + parameter.Key + "值：" + JsonConvert.SerializeObject(parameter.Value);
            }

            logManager.SaveInfo(log,null, AppType.TV,filterContext.ActionDescriptor.ActionName);
        }
    }
}