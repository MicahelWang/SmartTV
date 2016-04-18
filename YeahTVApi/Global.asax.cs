using YeahTVApi.App_Start;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.Infrastructure;
using YeahTVApi.Resx;
using YeahTVApi.Utilty;
using YeahTVApiLibrary.Infrastructure;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Timers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity.WebApi;

namespace YeahTVApi
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class Application : System.Web.HttpApplication
    {
        private static Timer timer = null;

        protected void Application_Start()
        {
            var logManager = UnityConfig.GetConfiguredContainer().Resolve<ILogManager>();
            logManager.SaveInfo("YeahApi Satrt", "YeahApi Satrt", AppType.TV);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(UnityConfig.GetConfiguredContainer());  

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            try
            {
                //设置初始化缓存
                SetCache();

                //设置初始化Timer
                SetTimer();
               
            }
            catch (Exception ex)
            {
                logManager.SaveError("set up model error", ex, AppType.TV);
            }
        }

        void Application_End(object sender, EventArgs e)
        {
            timer = null;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var context = new HttpContextWrapper(HttpContext.Current);
            var lastException = Server.GetLastError();

            LogAllErrors(context);
        }

        #region pravite method

        private void LogAllErrors(HttpContextBase context)
        {
           var logManager = UnityConfig.GetConfiguredContainer().Resolve<ILogManager>();
            var userName = context.User != null
                ? context.User.Identity.Name
                : string.Empty;

            if (context.AllErrors != null)
            {
                foreach (var exception in context.AllErrors)
                {
                    var errMsg = string.Format("Application Error : {0}, Client IP: {1}, Username: {2}",
                        exception.Message,
                        Request.UserHostAddress,
                        userName);
                    logManager.SaveError(errMsg, exception, AppType.TV);
                }
            }
        }

        private static void SetTimer()
        {
            if (timer == null)
                timer = new Timer();

            timer.Enabled = true;
            timer.Interval = Constant.CacheInterval;
            timer.Start();
            timer.Elapsed += (s, e) =>
            {
                var cacheManager = UnityConfig.GetConfiguredContainer().Resolve<ICacheManager>();
                var logManager = UnityConfig.GetConfiguredContainer().Resolve<ILogManager>();
                SetCache();

                logManager.SaveInfo("set timer cache", DateTime.Now.ToLongTimeString(), AppType.TV);
            };
        }

        private static void SetCache()
        {
            var cacheManager = UnityConfig.GetConfiguredContainer().Resolve<ICacheManager>();
            cacheManager.SetWeather();
            cacheManager.SetAppsList();
        }

        #endregion
    }
}
