using System.Net.Http;
using System.Reflection;
using System.Web.Http.Filters;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models.YeahHcsApi;
using YeahTVApi.Entity;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTVApiLibrary.Filter
{
    public class HCSTVApiApiErrorFilter : ExceptionFilterAttribute//ActionFilterAttribute, IExceptionFilter
    {
        private ILogManager logManager;

        public HCSTVApiApiErrorFilter(ILogManager logManager)
        {
            this.logManager = logManager;
        }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            string serverId;
            PropertyInfo property;

            var requestActionArguments = actionExecutedContext.ActionContext.ActionArguments["request"];

            property = requestActionArguments.GetType().GetProperty("Server_Id");
            serverId = property.GetValue(requestActionArguments, null).ToString();
            var message = string.Empty;

            if (actionExecutedContext.Exception is ApiException)
            {
                var apiException = actionExecutedContext.Exception as ApiException;
                message = (new ResponseData<object> { Data = new { ErrorMessage = apiException.Message, ErrorCode = (int)apiException.ExceptionType } }).ToJsonString();
            }
            else
            {
                message = (new ResponseData<object> { Data = new { ErrorMessage = actionExecutedContext.Exception.Message, ErrorCode = (int)ApiErrorType.System } }).ToJsonString();
            }

            // 写入日志
            logManager.SaveError(requestActionArguments.ToJsonString() + "\r\n" + actionExecutedContext.Exception.StackTrace + actionExecutedContext.Exception, "接口调用异常：" + actionExecutedContext.ActionContext.Request.RequestUri.OriginalString, AppType.HCS, serverId);
          
            actionExecutedContext.Response = new HttpResponseMessage() { Content = new StringContent(message) };

            base.OnException(actionExecutedContext);
        }
    }
}
