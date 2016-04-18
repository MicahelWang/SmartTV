using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YeahAppCentre.Web.Utility;
using YeahCenter.Infrastructure;
using YeahCenter.Infrastructure.WrapperFacadeInterface;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Infrastructure.ManagerInterface;

namespace YeahAppCentre.Controllers
{
    public class HCSCacheVersionController : BaseController
    {
        private readonly IHCSCacheVersionManager _hcsCacheVersionManager;
        private readonly IUserManager _manager;
        private readonly IGroupManager _groupManager;
        private readonly IHotelManager _hotelManager;
        private readonly IRoleManager _roleManager;
        private readonly IBrandManager _brandManager;
        private readonly IUerPermitionWrapperFacade _uerPermitionWrapperFacade;
        private readonly IHotelPermitionManager _hotelPermitionManager;
        public HCSCacheVersionController(IHCSCacheVersionManager hcsCacheVersionManager, IUerPermitionWrapperFacade uerPermitionWrapperFacade, IUserManager manager, IGroupManager groupManager, IHotelManager hotelManager, IRoleManager roleManager, IBrandManager brandManager, IHotelPermitionManager hotelPermitionManager)
        {
            this._uerPermitionWrapperFacade = uerPermitionWrapperFacade;
            this._brandManager = brandManager;
            this._manager = manager;
            _groupManager = groupManager;
            _hotelManager = hotelManager;
            _roleManager = roleManager;
            _hotelPermitionManager = hotelPermitionManager;
            this._hcsCacheVersionManager = hcsCacheVersionManager;
        }
        // GET: HCSCacheVersion
        public ActionResult Index()
        {
            return View();
        }

        [AjaxOnly]
        [HttpGet]
        public string GetTreeJson()
        {
            var resultTreeNodes = new List<TreeNode>();
            _groupManager.GetAll().ForEach(group => resultTreeNodes.Add(new HotelTreeNode()
            {
                drag = false,
                icon = "",
                id = group.Id,
                name = group.GroupName,
                open = false,
                pId = "0",
                PermitionType = "Group"

            }));
            _brandManager.GetAll().ForEach(brand => resultTreeNodes.Add(new HotelTreeNode()
            {
                drag = false,
                icon = "",
                id = brand.Id,
                name = brand.BrandName,
                open = false,
                pId = brand.GroupId,
                PermitionType = "Brand"

            }));
            _hotelManager.GetAllHotels().ForEach(hotel => resultTreeNodes.Add(new HotelTreeNode()
            {
                drag = false,
                icon = "",
                id = hotel.HotelId,
                name = hotel.HotelName,
                open = false,
                pId = hotel.BrandId,
                PermitionType = "Hotel"
            }));


            return JsonConvert.SerializeObject(resultTreeNodes).Replace("ischecked", "checked");
        }
        public ActionResult GetVersion(ViewHCSCache viewHCSCache)
        {
            return VersionHandle(viewHCSCache, (v =>
            {
                return _hcsCacheVersionManager.Search(new HCSCacheVersionCriteria()
                {
                    PermitionType = viewHCSCache.PermitionType,
                    TypeId = viewHCSCache.TypeId
                }).FirstOrDefault();
            }));
        }
        public ActionResult EditVersion(ViewHCSCache viewHCSCache)
        {
            return VersionHandle(viewHCSCache, (v =>
            {
                return _hcsCacheVersionManager.EditVersion(new HCSCacheVersionCriteria()
                {
                    PermitionType = v.PermitionType,
                    TypeId = v.TypeId
                }, CurrentUser.ChineseName);
            }));
        }

        private JsonResult VersionHandle(ViewHCSCache viewHCSCache, Func<ViewHCSCache, HCSCacheVersion> func)
        {
            var errorMsg = "";
            HCSCacheVersion version = null;
            if (viewHCSCache == null || string.IsNullOrWhiteSpace(viewHCSCache.TypeId) || string.IsNullOrWhiteSpace(viewHCSCache.PermitionType))
            {
                errorMsg = "请选择品牌或酒店!";
            }
            else
            {
                version = func(viewHCSCache);
            }
            return this.Json(new
            {
                ErrorMsg = errorMsg,
                LastUpdateTime = version != null ? version.LastUpdateTime.ToString() : "",
                Version = version != null ? version.Version : 1
            });
        }
    }
}