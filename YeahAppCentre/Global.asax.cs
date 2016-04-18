using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Elmah;
using Microsoft.Practices.ObjectBuilder2;
using YeahAppCentre.Controllers;
using YeahAppCentre.Web.Utility;
using YeahTVApi.Common;
using YeahTVApiLibrary.Manager;
using YeahTVApi.DomainModel.Enum;
using YeahTVApiLibrary.Infrastructure;

namespace YeahAppCentre
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie =
                Context.Request.Cookies[FormsAuthentication.FormsCookieName];

            //FormsCookieName
            if (authCookie != null && !string.IsNullOrWhiteSpace(authCookie.Value))
            {
                FormsAuthenticationTicket authTicket =
                    FormsAuthentication.Decrypt(authCookie.Value);
                string[] roles = authTicket.UserData.Split(new Char[] { ',' });
                GenericPrincipal userPrincipal =
                    new GenericPrincipal(new GenericIdentity(authTicket.Name),
                                         roles);
                Context.User = userPrincipal;
            }
        }
        protected void Application_Error()
        {
            if (HttpContext.Current == null || HttpContext.Current.Request.IsLocal)
            {
                // errors in Application_Start will end up here
                return;
            }
            HttpContextBase currentContext = new HttpContextWrapper(HttpContext.Current);
            UrlHelper urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            RouteData routeData = urlHelper.RouteCollection.GetRouteData(currentContext);
            string action = routeData.Values["action"] == null ? "" : routeData.Values["action"] as string;
            string controller = routeData.Values["controller"] == null ? "" : routeData.Values["controller"] as string;

            var exception = Server.GetLastError();


            var httpException = new HttpException(null, exception);
            if (httpException.GetHttpCode() == 404 & WebHelper.IsStaticResource(this.Request))
            {
                return;
            }

            var errorId = "";
            if (httpException.GetHttpCode() == 500)
            {
                errorId = DateTime.Now.ToString("yyyyMMddhhmmss");
                var addition = "";
                if (exception is System.Data.Entity.Validation.DbEntityValidationException)
                {
                    foreach (var errorItem in (exception as System.Data.Entity.Validation.DbEntityValidationException).EntityValidationErrors)
                    {
                        errorItem.ValidationErrors.ForEach(_ => addition += "PropertyName:" + _.PropertyName + _.ErrorMessage);
                    }
                    ErrorSignal.FromCurrentContext().Raise(new Exception(addition, exception));
                }

            }

            //TODO: 记录Log（忽略404，403） 
            var errorrouteData = new RouteData();
            errorrouteData.Values.Add("controller", "Error");
            errorrouteData.Values.Add("action", "Index");
            errorrouteData.Values.Add("errorId", errorId);
            errorrouteData.Values.Add("httpException", httpException);
            Server.ClearError();

            ILogManager logManager = DependencyResolver.Current.GetService<ILogManager>();
            logManager.SaveError(httpException, string.Format("全局错误:Global.asax,errorId:{0}", errorId), AppType.AppCenter, this.Request.Url.ToString());
            //TODO: 跳转到错误页面
            IController errorController = DependencyResolver.Current.GetService<ErrorController>();
            errorController.Execute(new RequestContext(new HttpContextWrapper(Context), errorrouteData));




        }

    }
}