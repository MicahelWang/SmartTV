using System.Web;
using System.Web.Mvc;
using YeahAppCentre.Web.Utility;
using YeahTVApiLibrary.Filter;
using YeahTVApiLibrary.Infrastructure;

namespace YeahAppCentre
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            var httpContextService = DependencyResolver.Current.GetService<IHttpContextService>();
            filters.Add(new HandleErrorAttribute());
            filters.Add(new JsonHandlerAttribute());
            filters.Add(new YeahAuthorizeAttribute(httpContextService));
        }
    }
}
