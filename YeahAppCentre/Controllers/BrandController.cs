using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YeahAppCentre.Web.Utility;
using YeahCenter.Infrastructure;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;

namespace YeahAppCentre.Controllers
{
    public class BrandController : BaseController
    {
        private readonly IBrandManager _brandManager;
        private readonly IGroupManager _groupManager;
        private IConstantSystemConfigManager constantSystemConfigManager;
        public BrandController(IBrandManager _brandManager,
        IGroupManager _groupManager, IConstantSystemConfigManager constantSystemConfigManager)
        {
            this._brandManager = _brandManager;
            this._groupManager = _groupManager;
            this.constantSystemConfigManager = constantSystemConfigManager;
        }

        public ActionResult Index(CoreSysBrandCriteria coreSysBrandCriteria = null)
        {
            if (coreSysBrandCriteria == null) coreSysBrandCriteria = new CoreSysBrandCriteria();
            coreSysBrandCriteria.NeedPaging = true;

            var partialViewResult = this.List(coreSysBrandCriteria) as PartialViewResult;
            if (partialViewResult != null)
                this.ViewBag.List = partialViewResult.Model;

            ViewBag.Groups = _groupManager.GetAll().ToSelectListItems();
            ViewBag.CoreSysBrandCriteria = coreSysBrandCriteria;

            return View();
        }

        [AjaxOnly]
        public ActionResult List(CoreSysBrandCriteria coreSysBrandCriteria)
        {
            var list = new PagedViewList<CoreSysBrand>();
            coreSysBrandCriteria.NeedPaging = true;

            list.PageIndex = coreSysBrandCriteria.Page;
            list.PageSize = coreSysBrandCriteria.PageSize;
            var source = _brandManager.Search(coreSysBrandCriteria);
            source.ForEach(m =>
            {
                m.Logo = constantSystemConfigManager.ResourceSiteAddress + m.Logo;
            });
            list.Source = source;

            list.TotalCount = coreSysBrandCriteria.TotalCount;
            return this.PartialView("List", list);
        }

        public ActionResult AddOrEdit(string id, OpType type)
        {
            var brand = new CoreSysBrand();
            ViewBag.OpType = type;
            ViewBag.Groups = _groupManager.GetAll().ToSelectListItems();
            switch (type)
            {
                case OpType.View:
                case OpType.Update:
                    brand = _brandManager.GetBrand(id);
                    break;
            }

            return PartialView(brand);
        }
        [HttpPost]
        public ActionResult AddOrEdit(OpType opType, CoreSysBrand dto)
        {
            return base.ExecutionMethod(() =>
            {
                string errorMsg = string.Empty;

                try
                {
                    if (string.IsNullOrWhiteSpace(dto.Logo))
                        errorMsg = "请上传LOGO图片！";
                    else
                    {
                        Uri u;
                        if (dto.Logo.ToLower().StartsWith("http") &&
                            Uri.TryCreate(dto.Logo, UriKind.RelativeOrAbsolute, out u))
                            dto.Logo = u.Segments[u.Segments.Length - 1];
                    }
                    

                    if (string.IsNullOrWhiteSpace(errorMsg))
                    {
                        switch (opType)
                        {
                            case OpType.Add:
                                _brandManager.Add(dto);
                                break;
                            case OpType.Update:
                                _brandManager.Update(dto);
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                }
                return this.Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
            });
        }
    }
}