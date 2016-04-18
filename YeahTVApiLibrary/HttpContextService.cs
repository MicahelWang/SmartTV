using System.Web;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTVApiLibrary
{
    public class HttpContextService : IHttpContextService
    {
        public HttpContextBase Current
        {
            get { return (new HttpContextWrapper(HttpContext.Current)); }
        }
    }
}