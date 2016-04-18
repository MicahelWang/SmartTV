namespace YeahTVApiLibrary.Filter
{
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.Entity;
    using YeahTVApiLibrary.Infrastructure;
    using Microsoft.Practices.Unity;
    using System;
    using System.Web;
    using System.Web.Mvc;

    public class HTApiErrorAttribute : ActionFilterAttribute, IExceptionFilter
    {

        private ILogManager logManager;
        public HTApiErrorAttribute(ILogManager logManager)
        {
            this.logManager = logManager;
        }

        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
                return;
            try
            {
                logManager.SaveError(
                     filterContext.Exception.StackTrace,
                    filterContext.Exception,
                    YeahTVApi.DomainModel.Enum.AppType.CommonFramework,
                    HttpContext.Current.Request.Url.ToString());
            }
            catch
            { 
                
            }

            ApiResult rst = new ApiResult();
            rst.Message = string.Empty;

            FunResult fr = GetExceptionResult(filterContext.Exception);
            rst.ResultType = fr.ResultType;
            rst.Message = fr.Message.Replace('\'', ' ');

            string publicKey = PubFun.ConvertToString(filterContext.HttpContext.Items[RequestParameter.ResultKey]);

            if (string.IsNullOrWhiteSpace(publicKey))
                publicKey = "TVUOqmnsnS8Oq8RWhuC93rKbR09T242V";

            var ecryptionModel = new EcryptionModel();
            ecryptionModel.apiResult = rst;
            ecryptionModel.apiResultFormat =  WebApiCommon.GetResultFormat(filterContext);
            ecryptionModel.PublicKey = publicKey;
            ecryptionModel.ecryptionType = YeahTVApi.DomainModel.EcryptionType.RC4;

            WebApiCommon.writenResponse(filterContext.HttpContext, ecryptionModel);

            filterContext.ExceptionHandled = true;
        }

        private FunResult GetExceptionResult(Exception exp)
        {
            FunResult rst = new FunResult();
            if (exp is ApiException)
            {
                ApiException e = exp as ApiException;
                rst.WithError(e.Message, e.ExceptionCode);
            }
            else
            {
                rst.WithError(exp.Message);
            }
            return rst;
        }
    }
}
