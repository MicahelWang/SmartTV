using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.Common;
using System.Runtime.Remoting.Contexts;
using System.Web.Security;

namespace YeahAppCentre.Web.Utility
{
    public class YeahAuthorizeAttribute : AuthorizeAttribute
    {
        private AuthorizationContext _authorizationContext;
        private readonly IHttpContextService _httpContextService;
        private bool _redirectLogin = false;
        private bool _redirectDashBoard = false;

        public YeahAuthorizeAttribute(IHttpContextService httpContextService)
        {
            this._httpContextService = httpContextService;
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        protected CurrentUser CurrentUser
        {
            get { return WebHelper.GetCurrentUser(_httpContextService.Current); }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            _authorizationContext = filterContext;
            base.OnAuthorization(filterContext);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            _redirectLogin = false;
            _redirectDashBoard = false;
            var routeData = _authorizationContext.RequestContext.RouteData;


            if (_authorizationContext.IsChildAction)
            {
                return true;
            }
            var controller = routeData.Values["controller"].ToString();
            var action = routeData.Values["action"].ToString();

            if (String.CompareOrdinal(controller, "Account") == 0
                || String.CompareOrdinal(controller, "Error") == 0
                || String.CompareOrdinal(controller, "Elmah") == 0)
            {
                return true;
            }

            var authorizeCore = base.AuthorizeCore(httpContext);
            if (authorizeCore == false)
            {
                _redirectLogin = true;
                return false;
            }
            if (string.IsNullOrEmpty(httpContext.User.Identity.Name))
            {
                _redirectLogin = true;
                return false;
            }

            var session = httpContext.Session;
            var request = _authorizationContext.RequestContext.HttpContext.Request;
            LoadSession(session, httpContext);
            if (request.IsAjaxRequest())
            {
                return true;
            }

            if (request.HttpMethod != "GET")
            {
                return true;
            }



            var fun = CurrentUser.FunList;

            //本地权限不做验证
            if (httpContext.Request.IsLocal)
            {
                //return true;
            }

            var types = new[] { "SRC", "PAGE" };
            var authorized = fun.Any(_ => types.Contains(_.Type) && _.Path != null && String.CompareOrdinal(controller, _.Controller) == 0 && String.CompareOrdinal(action, _.Action) == 0);

            if (authorized == false && (controller.ToLower() == "home" && action.ToLower() == "index") && (controller.ToLower() != "dashboard") && CurrentUser.UserType == (int)UserTypeEnum.Employee)
            {
                _redirectDashBoard = true;
            }

            return authorized;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (_redirectLogin)
            {
                if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = 498;
                    filterContext.Result = new ContentResult();
                }
                else
                {
                    base.HandleUnauthorizedRequest(filterContext);
                }

            }
            else
            {
                var defaultError = new RouteValueDictionary
                {
                    {"action", "Index"},
                    {"controller", "Error"},
                    {"httpCode", "403"}
                };

                if (_redirectDashBoard)
                {
                    defaultError = new RouteValueDictionary
                    {
                        {"action", "MoreHotelIndex"},
                        {"controller", "DashBoard"}
                    };
                }

                filterContext.Result = new RedirectToRouteResult(defaultError);
            }
        }


        private void LoadSession(HttpSessionStateBase session, HttpContextBase httpContext)
        {
            var currentUser = session[Constant.SessionKey.CurrentUser] as CurrentUser;
            if (currentUser == null)
            {
                var usersService = DependencyResolver.Current.GetService<IUserManager>();

                var authCookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                currentUser = usersService.GetCurrentUserData(httpContext.User.Identity.Name);
                currentUser.Token = authTicket.UserData;
                session.Add(Constant.SessionKey.CurrentUser, currentUser);
            }
        }
    }
}