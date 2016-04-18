using System.Web;
using System.Web.Mvc;

using Microsoft.Practices.Unity;

using YeahTVApiLibrary.Filter;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTvHcsApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
