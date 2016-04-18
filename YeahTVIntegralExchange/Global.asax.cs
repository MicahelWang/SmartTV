using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace YeahTVIntegralExchange
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
        protected void Application_Error(Object sender, EventArgs e)
        {
            //当路径出错，无法找到控制器时，不会执行FilterConfig中的OnException，而会在这里捕获。
            //当发生404错误时，执行完OnException后，还会执行到这里。
            //当发生其他错误，会执行OnException，但在base.OnException中已经处理完错误，不会再到这里执行。
            
            var lastError = Server.GetLastError();
            if (lastError != null)
            {
                
                var httpError = lastError as HttpException;
                if (httpError != null)
                {
                    //Server.ClearError();
                    switch (httpError.GetHttpCode())
                    {
                        case 404:
                            Response.Redirect("/JinJiang/IntegralExchange_Error");
                            break;
                        case 500:
                            Response.Redirect("/JinJiang/IntegralExchange_Error");
                            break; 
                    }
                }
            }
        }
    }
}
