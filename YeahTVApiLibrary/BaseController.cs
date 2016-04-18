namespace YeahTVApiLibrary.Controllers
{
    using YeahTVApi.Common;
    using YeahTVApi.Entity;
    using System.Globalization;
    using System.Web;
    using System.Web.Mvc;
using YeahTVApiLibrary.Infrastructure;
    using YeahTVApi.DomainModel.Enum;

    /// <summary>
    /// 基本控制器
    /// </summary>
    [ValidateInput(false)]
    public class BaseController : Controller
    {
        public IHttpContextService HttpContextService { get; set; }

        protected RequestHeader Header
        {
            get
            {
                var header = HttpContextService == null ? HttpContext.Items[RequestParameter.Header] : HttpContextService.Current.Items[RequestParameter.Header];

                return header as RequestHeader;
            }
        }

        protected TV_APPS TV_APPS
        {
            get
            {
                return HttpContext.Items[RequestParameter.APP] as TV_APPS;
            }
        }

        public BaseRequestData RequestData
        {
            get
            {
                var header = this.Header;
                var data = new BaseRequestData();
                data.Manufacturer = header.Manufacturer;
                data.devNo = header.DEVNO;
                data.language = header.Language;
                data.Brand = header.Brand;
                data.APP_ID = header.APP_ID;
                data.Model = header.Model;
                data.OSVersion = header.OSVersion;
                data.Platform = header.Platform;
                data.ver = header.Ver;
                return data;
            }
        }

        protected Guest MemberInfo
        {
            get
            {
                return Session[RequestParameter.Guest] as Guest;
            }

        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            CultureInfo culture = null;

            try
            {
                culture = new CultureInfo(Header.Language);
            }
            catch
            {
                culture = new CultureInfo("zh-CN");
            }

            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

            base.OnActionExecuting(filterContext);
        }
    }
}