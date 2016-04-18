using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;

using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.Common;

namespace YeahTVApiLibrary.Filter
{
    public class HCSApiErrorFilterAttribute : ExceptionFilterAttribute//ActionFilterAttribute, IExceptionFilter
    {
        private ILogManager logManager;

        public HCSApiErrorFilterAttribute(ILogManager logManager)
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

            // 写入日志
            logManager.SaveError(actionExecutedContext.Exception.StackTrace, actionExecutedContext.Exception, AppType.HCS, serverId);

            base.OnException(actionExecutedContext);
        }
    }
}
