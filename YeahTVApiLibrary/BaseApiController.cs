namespace YeahTVApiLibrary.Controllers
{
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using YeahTVApi.Common;
    using YeahTVApi.Entity;
    using YeahTVApiLibrary.Infrastructure;

    /// <summary>
    /// 基本控制器
    /// </summary>
    [ValidateInput(false)]
    public class BaseApiController : ApiController
    {
        public IHttpContextService HttpContextService { get; set; }

        protected RequestHeader Header
        {
            get
            {
                var header = HttpContext.Current.Items[RequestParameter.Header];

                return header as RequestHeader;
            }
        }

    }
}