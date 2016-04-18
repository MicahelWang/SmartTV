using Microsoft.Practices.Unity;
using System;
using System.Timers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity.WebApi;
using YeahAppCentre.Web.Utility;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Enum;
using YeahTVApiLibrary.Filter;
using YeahTVApiLibrary.Infrastructure;

namespace YeahHCSTVApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static Timer timer = null;

        protected void Application_Start()
        {
            var logManager = UnityConfig.GetConfiguredContainer().Resolve<ILogManager>();
            logManager.SaveInfo("YeahApi Satrt", "YeahApi Satrt", AppType.TV);

            Configure(GlobalConfiguration.Configuration);
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

        private void Configure(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.Filters.Add(new ElmahErrorAttribute());

            var logManager = UnityConfig.GetConfiguredContainer().Resolve<ILogManager>();
            httpConfiguration.Filters.Add(new HCSTVApiApiErrorFilter(logManager));
            httpConfiguration.Filters.Add(new YeahApiCheckSignFilterAttribute());
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
