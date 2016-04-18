using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using YeahAppCentre.Web.Utility;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahAppCentre.Controllers
{
    public class PartViewController : BaseController
    {
        private readonly IRoleManager _roleManager;
        private readonly IGroupManager _groupManager;
        private readonly IHotelManager _hotelManager;

        public PartViewController(IRoleManager roleManager,
            IGroupManager groupManager,
            IHotelManager hotelManager)
        {
            _roleManager = roleManager;
            _groupManager = groupManager;
            _hotelManager = hotelManager;
        }

        public ActionResult Menu(string controllerName, string actionName)
        {
            ViewBag.ControllerName = controllerName;
            ViewBag.ActionName = actionName;

            var fun = CurrentUser.FunList.Where(m => (m.Type == "SRC" || m.Type == "MODEL") && m.Display == 1).ToList();
            var currentFunc = CurrentUser.FunList.FirstOrDefault(m => m.Controller == controllerName && m.Action == actionName);
            if (currentFunc != null)
            {
                var resources = new List<ErpPowerResource>();
                while (currentFunc != null)
                {
                    resources.Add(currentFunc);
                    currentFunc = fun.FirstOrDefault(m => m.Id == currentFunc.ParentFuncId);
                }
                ViewBag.SelectParent = (resources.Count > 0) ? resources.Select(m => m.Id).ToArray() : new int[] { };
            }
            else
                ViewBag.SelectParent = new int[] { };
            return PartialView(fun);
        }

        public ActionResult Breadcrumbs(string controllerName, string actionName)
        {
            var currentUser = CurrentUser;
            var resourceList = _roleManager.GetPowerResource(currentUser.RoleId).ToList();
            var resource = resourceList.FirstOrDefault(m => m.Action == actionName && m.Controller == controllerName);
            if (resource != null)
            {
                var breadList = new List<ErpPowerResource> { resource };
                while (resource.ParentFuncId != 0)
                {
                    resource = resourceList.FirstOrDefault(m => m.Id == resource.ParentFuncId);
                    breadList.Add(resource);
                }
                return PartialView(breadList);

            }
            return PartialView();
        }


        public ActionResult Navbar()
        {
            ViewBag.DisplayName = CurrentUser.ChineseName;
            return PartialView();
        }

        public ActionResult DashBoardNavbar()
        {
            ViewBag.DisplayName = this.CurrentUser.ChineseName;
            return PartialView();
        }

        public ActionResult PageSetting()
        {
            return PartialView();
        }
        public ActionResult FilterHotels()
        {
            ViewBag.Groups = _groupManager.GetAll().ToSelectListItems();
            ViewBag.DefaultSelect = new List<SelectListItem> { DropDownExtensions.DefaultItem };
            return PartialView();
        }

        [AjaxOnly]
        public JsonResult SearchHotels(CoreSysHotelCriteria hotelCriteria)
        {
            var query=_hotelManager.GetAllCoreSysHotels().AsQueryable();
            if (!string.IsNullOrWhiteSpace(hotelCriteria.HotelName))
            {
                query = query.Where(m => m.HotelName.ToLower().Contains(hotelCriteria.HotelName.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(hotelCriteria.BrandId))
            {
                query = query.Where(m => m.BrandId.ToLower().Equals(hotelCriteria.BrandId.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(hotelCriteria.GroupId))
            {
                query = query.Where(m => m.GroupId.ToLower().Equals(hotelCriteria.GroupId.ToLower()));
            }

            var result=query.Select(m => new {m.Id, Name = m.HotelName});

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}