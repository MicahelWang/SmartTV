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
    public class OpenApiErrorFilterAttribute : ExceptionFilterAttribute//ActionFilterAttribute, IExceptionFilter
    {

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var requestActionArguments = actionExecutedContext.ActionContext.ActionArguments;

            string message;

            if (actionExecutedContext.Exception is ApiException)
            {
                var apiException = actionExecutedContext.Exception as ApiException;
                message = (new ResponseApiData<string> { Message = apiException.ToJsonString(), Code = (int)apiException.ExceptionType, Data = "" }).ToJsonString();
            }
            else
            {
                message = (new ResponseApiData<string> { Message = actionExecutedContext.Exception.ToJsonString(), Code = (int)ApiErrorType.System, Data = "" }).ToJsonString();
            }

            actionExecutedContext.Response = new HttpResponseMessage() { Content = new StringContent(message) };

            base.OnException(actionExecutedContext);
        }
    }
}
