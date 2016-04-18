using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using YeahAppCentre.Web.Utility;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahCenter.Infrastructure;
using YeahTVApiLibrary.Infrastructure;
using System.Net;
using YeahTVApi.DomainModel.Enum;
using YeahTVApiLibrary.Manager;
using System.Linq;
using YeahCentre.Manager;

namespace YeahAppCentre.Controllers
{
    public class AccountController : BaseController
    {
        private readonly HotelPermitionManager _hotelPermitionManager;
        public AccountController(HotelPermitionManager hotelPermitionManager)
        {
            _hotelPermitionManager = hotelPermitionManager;
        }

        [AllowAnonymous]
        // GET: Account
        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginModel loginModel, string returnUrl)
        {

            ViewResult loginViewResult = View("Index", loginModel);
            if (!ModelState.IsValid)
            {
                return loginViewResult;
            }

            string errorStr = "";
            var currentUser = SiteSecurityHelper.GetCurrentUserData(loginModel, ref errorStr);
            if (currentUser == null)
            {
                ModelState.AddModelError("", errorStr);
                return loginViewResult;
            }

            //设置Session
            Session.Clear();
            Session.Add(Constant.SessionKey.CurrentUser, currentUser);
            FormsAuthentication.SignOut();


            TimeSpan timespan = TimeSpan.FromMinutes(30);
            if (loginModel.RememberMe)
            {
                timespan = timespan.Add(TimeSpan.FromDays(7));
            }

            //写Cookie
            var authTicket = new FormsAuthenticationTicket(1, loginModel.UserName, DateTime.Now,
                                                           DateTime.Now.AddMinutes(timespan.TotalMinutes), loginModel.RememberMe,
                                                           currentUser.Token);
            string strTicket = FormsAuthentication.Encrypt(authTicket);
            var userCookie = new HttpCookie(FormsAuthentication.FormsCookieName, strTicket);
            if (authTicket.IsPersistent) { 
                userCookie.Expires = authTicket.Expiration;
            }
            Response.Cookies.Add(userCookie);
            //WriteLoginHistory();

            if (currentUser.UserType.HasValue && currentUser.UserType.Value == (int) UserTypeEnum.Employee)
            {
                var userHotelPermition = _hotelPermitionManager.GetHotelListByPermition(currentUser.UID);

                 if (userHotelPermition.Count == 0)
                    return Content(string.Format("<script>alert('{0}');window.location.href='{1}'; </script>","请联系管理员开通酒店权限！",Url.Action("Index","Account")));

                if (userHotelPermition.Count == 1)
                {
                    return RedirectToAction("Index", "DashBoard", new { hotelId = userHotelPermition.First().Id });
                }
        
                return userHotelPermition.Count > 1 ? RedirectToAction("MoreHotelIndex", "DashBoard") : RedirectToAction("Index", "Account");
            }
            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Account/LogOff


        public ActionResult LogOff()
        {
            SiteSecurityHelper.Logout();
            return RedirectToAction("Index", "Account");
        }

    }
}