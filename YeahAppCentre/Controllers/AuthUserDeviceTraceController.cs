using System;
using System.Linq;
using System.Web.Mvc;
using YeahAppCentre.Web.Utility;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;

namespace YeahAppCentre.Controllers
{
    public class AuthUserDeviceTraceController : BaseController
    {
        private IAuthUserDeviceTraceManager authUserDeviceTraceManager;
        private ILogManager logManager;
        private IUserManager usermannger;

        public AuthUserDeviceTraceController(
            IAuthUserDeviceTraceManager authUserDeviceTraceManager,
            ILogManager logManager,
            IUserManager usermannger)
        {
            this.authUserDeviceTraceManager = authUserDeviceTraceManager;
            this.logManager = logManager;
            this.usermannger = usermannger;
        }

        // GET: TVHotelConfig
        public ActionResult Index(AuthUserDeviceTraceCriteria authUserDeviceTraceCriteria = null)
        {
            if (authUserDeviceTraceCriteria == null)
            {
                authUserDeviceTraceCriteria = new AuthUserDeviceTraceCriteria();
                authUserDeviceTraceCriteria.NeedPaging = true;
            }
            var partialViewResult = this.List(authUserDeviceTraceCriteria) as PartialViewResult;
            if (partialViewResult != null)
                this.ViewBag.List = partialViewResult.Model;
            ViewBag.authUserDeviceTraceCriteria = authUserDeviceTraceCriteria;



            if (!string.IsNullOrEmpty(authUserDeviceTraceCriteria.UserId))
            {
                var UserName = usermannger.GetEntity(authUserDeviceTraceCriteria.UserId).UserName;


                ViewBag.UserName = UserName;
            }
            return View();
        }
        [AjaxOnly]
        public ActionResult List(AuthUserDeviceTraceCriteria authUserDeviceTraceCriteria)
        {
            var list = new PagedViewList<AuthUserDeviceTrace>();
            authUserDeviceTraceCriteria.NeedPaging = true;

            list.PageIndex = authUserDeviceTraceCriteria.Page;
            list.PageSize = authUserDeviceTraceCriteria.PageSize;
            list.Source = authUserDeviceTraceManager.SearhAuthUserDeviceTrace(authUserDeviceTraceCriteria);
            list.TotalCount = authUserDeviceTraceCriteria.TotalCount;

            return this.PartialView("List", list);
        }

        [HttpGet]

        public ActionResult AddOrEdit(string id, OpType type)
        {
            ViewBag.OpType = type;
            var authUserDeviceTrace = new AuthUserDeviceTrace();
            switch (type)
            {

                case OpType.Add:
                    break;
                case OpType.View:
                case OpType.Update:
                    authUserDeviceTrace = authUserDeviceTraceManager.GetEntity(id);
                    break;
                default:
                    break;
            }

            return PartialView(authUserDeviceTrace);

        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult AddOrEdit(OpType optype, AuthUserDeviceTrace authUserDeviceTrace)
        {
            return base.ExecutionMethod(() =>
            {
                string errorMsg = string.Empty;

                if (optype == OpType.Add)
                {
                    var exitauthUserDeviceTrace = authUserDeviceTraceManager.SearhAuthUserDeviceTrace(new AuthUserDeviceTraceCriteria { UserId = authUserDeviceTrace.UserId, DeviceNo = authUserDeviceTrace.DeviceNo });
                    if (exitauthUserDeviceTrace != null && exitauthUserDeviceTrace.Any(e => e.DeviceNo == authUserDeviceTrace.DeviceNo))
                    {
                        errorMsg = "用户名已存在";

                    }
                    else
                    {
                        authUserDeviceTrace.BindTime = DateTime.Now;
                        authUserDeviceTrace.LastVisitTime = DateTime.Now;
                        authUserDeviceTraceManager.AddAuthUserDeviceTrace(authUserDeviceTrace);
                    }

                }
                else
                {
                    authUserDeviceTrace.BindTime = DateTime.Now;
                    authUserDeviceTrace.LastVisitTime = DateTime.Now;
                    authUserDeviceTraceManager.UpdateAuthUserDeviceTrace(authUserDeviceTrace);
                }

                return this.Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
            });
        }

        //public string GetHotelNameById(string hotelId)
        //{
        //    if (hotelManager.GetHotelObject(hotelId).Hotel != null)
        //    {
        //        return hotelManager.GetHotelObject(hotelId).Hotel.HotelName;
        //    }
        //    return "";
        //}
        public ActionResult Delete(AuthUserDeviceTrace authUserDeviceTrace)
        {
            return base.ExecutionMethod(() =>
           {
               string errorMsg = string.Empty;


               authUserDeviceTraceManager.DeleteAuthUserDeviceTrace(authUserDeviceTrace);

               return this.Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
           });

        }

        public string GetUserNameByUserId(string userId)
        {
            var result = "";
            if (!string.IsNullOrWhiteSpace(userId))
            {
                var userEntity = usermannger.GetEntity(userId);
                result = userEntity == null ? "" : userEntity.UserName;
            }
            return result;
        }

    }
}