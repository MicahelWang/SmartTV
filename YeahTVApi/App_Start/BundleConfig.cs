using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace YeahTVApi.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery.min.js",
                       "~/Scripts/jquery.mobile-1.4.3.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/selfService.css"));
        }
    }
}