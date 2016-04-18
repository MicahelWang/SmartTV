using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity.WebApi;
using YeahAppCentre.Web.Utility;

namespace YeahCentreApi
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            Configure(GlobalConfiguration.Configuration);
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(UnityConfig.GetConfiguredContainer());  

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void Configure(HttpConfiguration httpConfiguration)
        {

            httpConfiguration.Filters.Add(
                new ElmahErrorAttribute()
            );
        }
    }
}
