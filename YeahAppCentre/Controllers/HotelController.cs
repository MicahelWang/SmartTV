using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using YeahAppCentre.Web.Utility;
using YeahCenter.Infrastructure;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;

namespace YeahAppCentre.Controllers
{
    public class HotelController : BaseController
    {
        private readonly IHotelManager _manager;
        private readonly ITvTemplateManager _tvTemplateManager;
        private readonly IBrandManager _brandManager;
        private readonly IGroupManager _groupManager;
        private readonly IProvinceManager _provinceManager;
        private readonly ICityManager _cityManager;
        private readonly ICountyManager _countyManager;
        public HotelController(
            IHotelManager manager,
            ITvTemplateManager tvTemplateManager,
            IBrandManager brandManager,
            IGroupManager groupManager,
            IProvinceManager provinceManager,
            ICityManager cityManager,
            ICountyManager countyManager,
            IHttpContextService httpContextService)
        {
            _manager = manager;
            _tvTemplateManager = tvTemplateManager;
            _brandManager = brandManager;
            _groupManager = groupManager;
            _provinceManager = provinceManager;
            _cityManager = cityManager;
            _countyManager = countyManager;
        }
        // GET: Hotel
        public ActionResult Index(CoreSysHotelCriteria hotelCriteria = null)
        {
            if (hotelCriteria == null) hotelCriteria = new CoreSysHotelCriteria();
            hotelCriteria.NeedPaging = true;

            var partialViewResult = List(hotelCriteria) as PartialViewResult;
            if (partialViewResult != null)
                ViewBag.List = partialViewResult.Model;

            ViewBag.HotelCriteria = hotelCriteria;
            ViewBag.Groups = _groupManager.GetAll().ToSelectListItems();
            ViewBag.DefaultSelect = new List<SelectListItem>() { DropDownExtensions.DefaultItem };

            return View();
        }

        [AjaxOnly]
        public ActionResult List(CoreSysHotelCriteria hotelCriteria)
        {
            if (hotelCriteria == null) hotelCriteria = new CoreSysHotelCriteria();
            var list = new PagedViewList<CoreSysHotel>();
            hotelCriteria.NeedPaging = true;
            list.PageIndex = hotelCriteria.Page;
            list.PageSize = hotelCriteria.PageSize;
            list.Source = _manager.Search(hotelCriteria);
            list.TotalCount = hotelCriteria.TotalCount;

            return this.PartialView("List", list);
        }

        /// <summary>
        /// 新增 查看及修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(string id, OpType type)
        {
            CoreSysHotel hotelEntity;
            ViewBag.BaseDate = _manager.GetDataBase(id).JsonStringToObj<ViewCoreSysHotel>() ?? new ViewCoreSysHotel();
            ViewBag.OpType = type;
            ViewBag.Templates = _tvTemplateManager.GetAll().ToSelectListItems();
            ViewBag.CountyItems = _countyManager.GetAll().ToSelectListItems();

            if (type == OpType.Add)
            {
                hotelEntity = new CoreSysHotel();
            }
            else
            {
                hotelEntity = _manager.GetCoreSysHotelById(id);
                var county = _countyManager.GetAll().ToSelectListItems().FirstOrDefault(m => m.Value == hotelEntity.Country.ToString());
                ViewBag.CountyName = county == null ? "未选择地区" : county.Text;
            }
            hotelEntity = hotelEntity ?? new CoreSysHotel();
            return View(hotelEntity);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateInput(false)] 
        public ActionResult Edit(OpType opType, CoreSysHotel dto)
        {
            ViewBag.OpType = opType;
            var errorMsg = string.Empty;
            if (string.IsNullOrEmpty(dto.HotelName))
            {
                errorMsg = "酒店名称不能为空！";
            }
            else if (dto.CoreSysHotelSencond == null)
            {
                errorMsg = "扩展信息不能为空！";
            }
            else if (dto.Country == 0)
            {
                errorMsg = "请选择地区！";
            }
            else if (string.IsNullOrWhiteSpace(dto.BrandId))
            {
                errorMsg = "请选择品牌！";
            }

            if (string.IsNullOrEmpty(errorMsg))
            {
                var brandEntity = _brandManager.GetBrand(dto.BrandId);
                var countyEntity = _countyManager.GetById(dto.Country);
                var cityEntity = _cityManager.GetById(countyEntity.ParentId);
                dto.GroupId = _groupManager.GetAll().First(m => m.Id == brandEntity.GroupId).Id;
                dto.City = cityEntity.Id;
                dto.Province = _provinceManager.GetById(cityEntity.ParentId).Id;

                switch (opType)
                {
                    case OpType.Add:
                        var exitCoreSysHotel = _manager.GetCoreSysHotelByname(dto.HotelName);
                        
                            if (exitCoreSysHotel != null)
                            {
                                errorMsg = "该酒店名称已存在";
                            }
      
                            else
                            {
                                dto.Id = Guid.NewGuid().ToString("N");

                                var brandCode = brandEntity.BrandCode;
                                var sameBrandHotelCount = (_manager.GetSameBrandHotelCount(brandEntity.Id) + 1).ToString().PadLeft(3, '0');
                                dto.HotelCode = string.Format("{0}{1}", brandCode, sameBrandHotelCount);
                                dto.CreateTime = DateTime.Now;
                                if (dto.IsLocalPMS == false)
                                    dto.CoreSysHotelSencond.LocalPMSUrl = null;
                                _manager.Add(dto);
                            }
                        break;
                    case OpType.Update:
                        if (dto.IsLocalPMS == false)
                            dto.CoreSysHotelSencond.LocalPMSUrl = null;
                        _manager.Update(dto);
                        break;
                }
            }

            return Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
        }

        [AjaxOnly]
        public ActionResult BatchDelete(string id)
        {
            var idArray = id.Split(',');
            if (idArray.Length > 0)
                _manager.BatchDelete(idArray);
            return Content("Success");
        }

        #region SelectListItem

        [HttpGet]
        [AjaxOnly]
        public JsonResult GetAreasJson()
        {
            var resultTreeNodes = new List<TreeNode>();
            _countyManager.GetAll().ForEach(county => resultTreeNodes.Add(new TreeNode()
            {
                drag = false,
                icon = "",
                id = county.Id.ToString(),
                name = county.Name,
                ischecked = false,
                open = false,
                pId = "city" + county.ParentId.ToString()

            }));
            _cityManager.GetAll().ForEach(city => resultTreeNodes.Add(new TreeNode()
            {
                drag = false,
                icon = "",
                id = "city" + city.Id.ToString(),
                name = city.Name,
                ischecked = false,
                open = false,
                pId = "province" + city.ParentId.ToString()

            }));
            _provinceManager.GetAll().ForEach(province => resultTreeNodes.Add(new TreeNode()
            {
                drag = false,
                icon = "",
                id = "province" + province.Id.ToString(),
                name = province.Name,
                ischecked = false,
                open = false,
                pId = "0"

            }));

            return Json(resultTreeNodes, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AjaxOnly]
        public JsonResult GetBrandsJson()
        {
            var resultTreeNodes = new List<TreeNode>();
            _brandManager.GetAll().ForEach(brand => resultTreeNodes.Add(new TreeNode()
            {
                drag = false,
                icon = "",
                id = brand.Id,
                name = brand.BrandName,
                ischecked = false,
                open = false,
                pId = brand.GroupId

            }));
            _groupManager.GetAll().ForEach(group =>
            {
                if (resultTreeNodes.Find(m => m.pId == group.Id) != null)
                    resultTreeNodes.Add(new TreeNode()
                    {
                        drag = false,
                        icon = "",
                        id = group.Id,
                        name = group.GroupName,
                        ischecked = false,
                        open = false,
                        pId = "0"

                    });
            });
            return Json(resultTreeNodes, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AjaxOnly]
        public JsonResult GetBrandsByGroup(string id)
        {
            var result = _brandManager.GetBrandsByGroup(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
