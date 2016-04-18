using System.Net.Http;
using System.Reflection;
using System.Web.Http.Filters;
using Microsoft.Practices.Unity;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models.PointsModels;
using YeahTVApi.DomainModel.Models.YeahHcsApi;
using YeahTVApi.Entity;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTVApiLibrary.Filter
{
    public class TvApiErrorFilterAttribute : ExceptionFilterAttribute//ActionFilterAttribute, IExceptionFilter
    {
        [Dependency]
        public ILogManager logManager { get; set; }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var requestActionArguments = actionExecutedContext.ActionContext.ActionArguments;

            string message;

            if (actionExecutedContext.Exception is ApiException)
            {
                var apiException = actionExecutedContext.Exception as ApiException;
                message = (new ResponseApiData<string> { Message = apiException.Message, Code = (int)apiException.ExceptionType, Data = "" }).ToJsonString();
            }
            else
            {
                message = (new ResponseApiData<string> { Message = actionExecutedContext.Exception.Message, Code = (int)ApiErrorType.System, Data = "" }).ToJsonString();
            }

            logManager.SaveError(requestActionArguments.ToJsonString() + "\r\n" + actionExecutedContext.Exception.StackTrace + actionExecutedContext.Exception, "接口调用异常：" + actionExecutedContext.ActionContext.Request.RequestUri.OriginalString, AppType.TV);

            actionExecutedContext.Response = new HttpResponseMessage() { Content = new StringContent(message) };

            base.OnException(actionExecutedContext);
        }
    }
}
