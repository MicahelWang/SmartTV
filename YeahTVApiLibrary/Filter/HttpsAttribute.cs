namespace YeahTVApiLibrary.Filter
{
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel;
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filterContext"></param>
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpsAttribute : RequireHttpsAttribute
    {
        public bool AllowHttpRequest { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (AllowHttpRequest)
            {
                return;
            }

            if (filterContext.HttpContext.Request.IsSecureConnection)
            {
                return;
            }

            if (string.Equals(filterContext.HttpContext.Request.Headers["X-Forwarded-Proto"],
                "https",
                StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            HandleNonHttpsRequest(filterContext);
        }

        protected override void HandleNonHttpsRequest(AuthorizationContext filterContext)
        {
            base.HandleNonHttpsRequest(filterContext);

            var result = (RedirectResult)filterContext.Result;

            var uri = new UriBuilder(result.Url);
            uri.Port = Constant.HttpsPort;

            filterContext.Result = new RedirectResult(uri.ToString());
        }
    }

}