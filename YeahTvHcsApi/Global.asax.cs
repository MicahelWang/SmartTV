using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Unity.WebApi;
using YeahAppCentre.Web.Utility;
using YeahTVApiLibrary.Filter;
using Microsoft.Practices.Unity;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTvHcsApi
{
    public class WebApiApplication : System.Web.HttpApplication
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

            httpConfiguration.Filters.Add(new ElmahErrorAttribute());

            var logManager = UnityConfig.GetConfiguredContainer().Resolve<ILogManager>();
            httpConfiguration.Filters.Add(new HCSApiErrorFilterAttribute(logManager));
        }
    }
}
