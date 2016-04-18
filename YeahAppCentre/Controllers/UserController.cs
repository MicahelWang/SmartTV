using System;
using System.Collections.Generic;
using System.Web.Mvc;
using YeahAppCentre.Models;
using YeahAppCentre.Web.Utility;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ConditionModels;
using YeahCenter.Infrastructure;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.DomainModel.Models.ViewModels;
using Newtonsoft.Json;
using YeahTVApi.DomainModel.Models.DomainModels;
using System.Linq;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahCenter.Infrastructure.WrapperFacadeInterface;

namespace YeahAppCentre.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserManager _manager;
        private readonly IGroupManager _groupManager;
        private readonly IHotelManager _hotelManager;
        private readonly IRoleManager _roleManager;
        private readonly IBrandManager _brandManager;
        private readonly IUerPermitionWrapperFacade _uerPermitionWrapperFacade;
        private readonly IHotelPermitionManager _hotelPermitionManager;

        public UserController(IUerPermitionWrapperFacade uerPermitionWrapperFacade, IUserManager manager, IGroupManager groupManager, IHotelManager hotelManager, IRoleManager roleManager, IBrandManager brandManager, IHotelPermitionManager hotelPermitionManager)
        {
            this._uerPermitionWrapperFacade = uerPermitionWrapperFacade;
            this._brandManager = brandManager;
            this._manager = manager;
            _groupManager = groupManager;
            _hotelManager = hotelManager;
            _roleManager = roleManager;
            _hotelPermitionManager = hotelPermitionManager;
        }

        // GET: User
        public ActionResult Index(UserCondition condition = null)
        {
            if (condition == null)
            {
                condition = new UserCondition();
            }
            var conditionViewResult = this.Condition(condition) as PartialViewResult;
            if (conditionViewResult != null)
                this.ViewBag.Condition = conditionViewResult.Model;
            var partialViewResult = this.List(condition) as PartialViewResult;
            if (partialViewResult != null)
                this.ViewBag.List = partialViewResult.Model;
            return View(condition);
        }

        [HttpGet]
        public ActionResult EditeIndex(string id, OpType type)
        {
            ViewBag.Id = id;
            ViewBag.Type = type;
            return View();
        }




        public string GetTreeJson(string id, string type)
        {
            var resultTreeNodes = new List<TreeNode>();
            var listPermition = GetListPermition(id);
            var groups = listPermition.Where(m => m.PermitionType == "Group").Select(m=>m.TypeId).ToList();
            var brands = listPermition.Where(m => m.PermitionType == "Brand").Select(m => m.TypeId).ToList();
            var hotels = listPermition.Where(m => m.PermitionType == "Hotel").Select(m => m.TypeId).ToList();

            var selectGroups = _brandManager.GetAll().Where(m => brands.Contains(m.Id)).Select(m=>m.GroupId);

            var hotelQuery=_hotelManager.GetAllHotels().Where(m => hotels.Contains(m.HotelId));
            var hotelgroups=(hotelQuery.Select(m => m.GroupId).ToList());
            var hotelbrands=(hotelQuery.Select(m => m.BrandId).ToList());

            _groupManager.GetAll().ForEach(group => resultTreeNodes.Add(new TreeNode()
            {
                drag = false,
                icon = "",
                id = group.Id,
                name = group.GroupName,
                ischecked = groups.Any(m => m.Equals(group.Id)) || hotelgroups.Any(m => m.Equals(group.Id)) || selectGroups.Any(m => m.Equals(group.Id)),
                open = false,
                pId = "0"

            }));
            _brandManager.GetAll().ForEach(brand => resultTreeNodes.Add(new TreeNode()
            {
                drag = false,
                icon = "",
                id = brand.Id,
                name = brand.BrandName,
                ischecked = groups.Any(m => m.Equals(brand.GroupId)) || brands.Any(m => m.Equals(brand.Id)) || hotelbrands.Any(m => m.Equals(brand.Id)),
                open = false,
                pId = brand.GroupId

            }));
            _hotelManager.GetAllHotels().ForEach(hotel => resultTreeNodes.Add(new TreeNode()
            {
                drag = false,
                icon = "",
                id = hotel.HotelId,
                name = hotel.HotelName,
                ischecked = groups.Any(m => m.Equals(hotel.GroupId)) || brands.Any(m => m.Equals(hotel.BrandId)) || hotels.Any(m => m.Equals(hotel.HotelId)),
                open = false,
                pId = hotel.BrandId
            }));


            return JsonConvert.SerializeObject(resultTreeNodes).Replace("ischecked", "checked");
        }


        public List<HotelPermition> GetListPermition(string userId)
        {
            return _hotelPermitionManager.GetHotelByUserId(userId);;
        }


        [HttpGet]
        public ActionResult Edit(string id, OpType type)
        {

            var group = _groupManager.GetAll().ToSelectListItems();
            var roles = _roleManager.GetAll().ToSelectListItems();
            ViewBag.Groups = group;
            ViewBag.Roles = roles;
            ViewBag.DefaultSelect = new List<SelectListItem>() { DropDownExtensions.DefaultItem };
            ViewBag.OpType = type;

            switch (type)
            {
                case OpType.Update:
                case OpType.View:
                    var model = _manager.GetEntity(id);
                    return PartialView(model);
            }
            return PartialView();

        }

        // POST: Role/Edit/5
        [HttpPost]
        [AjaxOnly]
        public ActionResult Edit(OpType opType, string permition, ErpSysUser dto)
        {
            List<HotelPermition> lists = new List<HotelPermition>();
            if (!string.IsNullOrEmpty(permition))
            {
                lists = permition.JsonStringToObj<List<HotelPermition>>();
            }
            var errorMsg = string.Empty;
            var currentUser = CurrentUser.UID;
            dto.ModifyDate = DateTime.Now;
            dto.ModifyUser = currentUser;
            switch (opType)
            {
                case OpType.Add:
                    dto.CreateDate = DateTime.Now;
                    dto.CreateUser = currentUser;
                    dto.Id = Guid.NewGuid().ToString("N").ToUpper();
                    foreach (var item in lists)
                    {
                        item.Id = Guid.NewGuid().ToString("N").ToUpper();
                        item.UserId = dto.Id;
                    }
                    _uerPermitionWrapperFacade.AddPermintion(lists, dto);
                    break;
                case OpType.Update:
                    foreach (var item in lists)
                    {
                        item.Id = Guid.NewGuid().ToString("N").ToUpper();
                        item.UserId = dto.Id;
                    }
                    _uerPermitionWrapperFacade.UpdatePermintion(lists,dto);
                    break;
            }

            return this.Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
        }

        public ActionResult Condition(UserCondition condition)
        {
            var group = _groupManager.GetAll().ToSelectListItems();
            var roles = _roleManager.GetAll().ToSelectListItems();
            ViewBag.Groups = group;
            ViewBag.Roles = roles;
            ViewBag.DefaultSelect = new List<SelectListItem>() { DropDownExtensions.DefaultItem };
            return PartialView(condition);
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult EditLogin(string id, OpType type)
        {

            ViewBag.OpType = type;

            switch (type)
            {
                case OpType.Update:
                case OpType.View:
                    var model = _manager.GetLoginAccount(id);
                    return PartialView(model);
            }
            return PartialView();

        }

        // POST: Role/Edit/5
        [HttpPost]
        [AjaxOnly]
        public ActionResult EditLogin(OpType opType, CoreSysLogin dto)
        {
            var errorMsg = string.Empty;
            switch (opType)
            {
                case OpType.Update:
                    _manager.UpdateLoginAccount(dto);
                    break;
            }

            return this.Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
        }
        [HttpGet]
        [AjaxOnly]
        public ActionResult Register(string id)
        {
            var registerModel = new RegisterViewModel { UserId = id };
            return PartialView(registerModel);
        }
        [AjaxOnly]
        [HttpPost]
        public ActionResult Register(RegisterViewModel registerModel)
        {
            var errorMsg = string.Empty;
            var dto = new CoreSysLogin
            {
                Id = registerModel.UserId,
                RegDate = DateTime.Now,
                IsDelete = false,
                LoginName = registerModel.LoginName,
                UserName = registerModel.LoginName,
                AddUserId = CurrentUser.UID,
                State = StateEnum.Normal.ToInt(),
                Password = registerModel.Password
            };
            _manager.AddLoginAccount(dto);
            return this.Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
        }

        [AjaxOnly]
        public ActionResult List(UserCondition condition, int page = 0)
        {
            const int pageSize = 10;
            var list = this._manager.PagedList(page, pageSize, condition);
            return this.PartialView("List", list);
        }

        [HttpGet]
        [AjaxOnly]
        public JsonResult GetHotelByGroup(string id)
        {
            var result = _hotelManager.GetByGroup(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult BatchDelete(string id)
        {
            var errorMsg = string.Empty;
            var userIds = id.Split(',');
            _manager.BatchDelete(userIds);
            return this.Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
        }
    }
}