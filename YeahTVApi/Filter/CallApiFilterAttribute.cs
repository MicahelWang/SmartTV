namespace YeahTVApi.Filter
{
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel;
    using YeahTVApi.Entity;
    using System;
    using System.Web;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Method)]
    public class CallApiFilterAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            ApiResultFormat fmt = WebApiCommon.GetResultFormat(filterContext);
            if (filterContext.Result is ApiResult)
            {
                ApiResult apiResult = filterContext.Result as ApiResult;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.Write(apiResult.ToJsonString());
            }
            base.OnResultExecuted(filterContext);
        }
    }
}