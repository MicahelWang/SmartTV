using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using YeahTVApi.Common;
using System.Dynamic;

namespace YeahTVApi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
           
            DynamicObj con = new DynamicObj();
            string str = string.Empty;

            str = PubFun.GetAppSetting("app.route.controller");
            if (!string.IsNullOrEmpty(str)) con["controller"] = str;

            str = PubFun.GetAppSetting("app.route.action");
            if (!string.IsNullOrEmpty(str)) con["action"] = str;

            str = PubFun.GetAppSetting("app.route.format");
            if (!string.IsNullOrEmpty(str)) con["format"] = str;
           // routes.MapRoute("debug", "local/{controller}/{action}/debug", null, con);
            //routes.MapRoute("local_api_json", "local/{controller}/{action}", null, null);

            //routes.MapRoute("test", "{controller}/{action}");

            routes.MapRoute("Defualt", "{controller}/{action}/");
            routes.IgnoreRoute("favicon.ico");
        }
    }
}