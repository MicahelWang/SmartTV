using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.Common;
using YeahTVApiLibrary.Infrastructure;

namespace YeahAppCentre.Web.Utility
{
    public static class WebHelper
    {
        // <summary>
        /// Returns true if the requested resource is one of the typical resources that needn't be processed by the cms engine.
        /// </summary>
        /// <param name="request">HTTP Request</param>
        /// <returns>True if the request targets a static resource file.</returns>
        /// <remarks>
        /// These are the file extensions considered to be static resources:
        /// .css
        ///	.gif
        /// .png 
        /// .jpg
        /// .jpeg
        /// .js
        /// .axd
        /// .ashx
        /// </remarks>
        public static bool IsStaticResource(HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            string path = request.Path;
            string extension = VirtualPathUtility.GetExtension(path);

            if (extension == null) return false;

            switch (extension.ToLower())
            {
                case ".axd":
                case ".ashx":
                case ".bmp":
                case ".css":
                case ".gif":
                case ".htm":
                case ".html":
                case ".ico":
                case ".jpeg":
                case ".jpg":
                case ".js":
                case ".png":
                case ".rar":
                case ".zip":
                    return true;
            }

            return false;
        }
        public static CurrentUser GetCurrentUser(HttpContextBase httpContext)
        {
            var session = httpContext.Session;
            return session[Constant.SessionKey.CurrentUser] as CurrentUser;
        }

        public static string GetShoppingOrderAddress(this UrlHelper urlHelper, string hotelId)
        {
            if (string.IsNullOrEmpty(hotelId))
                return string.Empty;

            var user = GetCurrentUser(DependencyResolver.Current.GetService<IHttpContextService>().Current);
            var constantSystemConfigManager = DependencyResolver.Current.GetService<IConstantSystemConfigManager>();

            return string.Format("{3}?userName={0}&token={1}&hotelId={2}", user.Account, user.Token, hotelId, constantSystemConfigManager.ShoppingOrderAddress);
        }

        public static bool IsContains<TSource>(this IEnumerable<TSource> source, TSource value)
        {
            return source != null && source.Contains(value);
        }

        public static string ToDisplayString(this string source, int maxLength = 50)
        {
            return source.Length > maxLength ? source.Substring(0, maxLength) : source;
        }



        public static Tuple<decimal, string> ToSizeWithUnit(this int sourceSize)
        {
            decimal size = sourceSize;
            int count = 0;
            while (size > 1024)
            {
                count++;
                size = size / 1024;
            }
            var unitStr = ((UnitEnum)count).GetText();
            return new Tuple<decimal, string>(size, unitStr);
        }

    }
    enum UnitEnum
    {
        KB = 1,
        MB = 2,
        GB = 3,
        TB = 4,
    }
}
