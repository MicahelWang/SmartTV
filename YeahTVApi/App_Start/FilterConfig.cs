namespace YeahTVApi
{
    using System.Web;
    using System.Web.Mvc;
    using YeahTVApi.Filter;
    using YeahTVApiLibrary.Filter;
    using YeahTVApi.App_Start;
    using YeahTVApiLibrary.Infrastructure;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// 
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            var logManager = UnityConfig.GetConfiguredContainer().Resolve<ILogManager>();
            filters.Add(new HTApiErrorAttribute(logManager));
            filters.Add(new HttpsAttribute());
            filters.Add(new JsonHandlerAttribute());
            //filters.Add(new AppLogAttribute());
        }
    }
}