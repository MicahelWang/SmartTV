using System.Web;
using System.Web.Optimization;
using YeahAppCentre.Web.Utility;
using YeahOnlieShoppingMall.App_Start;
using System.Collections.Generic;

namespace YeahOnlieShoppingMall
{
    public class CustomBundleConfig
    {
        public static void RegisterCustomBundles()
        {
            CustomBundleCollection.IncludeStyle("~/Content/ace/css", "http://resource.yeah-info.com", "~/Content/Ace/css/bootstrap.min.css");
            CustomBundleCollection.IncludeScript("~/Content/ace/script", "http://resource.yeah-info.com",
                "~/Content/Ace/js/jquery-1.10.2.min.js",
                 "~/Content/Ace/js/bootstrap.min.js");
        }
    }
}
