using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using YeahAppCentre.Web.Utility;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models.DataModel;

namespace YeahAppCentre.Controllers
{
    public class TemplateController : BaseController
    {
        private readonly ITvTemplateManager _manager;
        private readonly ITvDocumentElementManager _elementManager;
        private readonly ITvDocumentAttributeManager _attributeManager;
        private readonly ITvTemplateTypeManager _templateTypeManager;
        private readonly ITvTemplateElementManager _templateElementManager;
        private readonly IHotelManager _hotelManager;


        public TemplateController(
            ITvTemplateManager manager,
            ITvDocumentElementManager elementManager,
            ITvDocumentAttributeManager attributeManager,
            ITvTemplateTypeManager templateTypeManager,
            ITvTemplateElementManager templateElementManager,
            IHotelManager hotelManager)
        {
            _manager = manager;
            _elementManager = elementManager;
            _attributeManager = attributeManager;
            _templateTypeManager = templateTypeManager;
            _templateElementManager = templateElementManager;
            _hotelManager = hotelManager;
        }

        // GET: Template
        public ActionResult Index()
        {
            var partialViewResult = List() as PartialViewResult;
            if (partialViewResult != null)
                ViewBag.List = partialViewResult.Model;
            ViewBag.keyWord = "";
            return View();
        }

        [AjaxOnly]
        public ActionResult List(string keyword = "", int page = 0)
        {
            const int pageSize = 10;
            var list = _manager.PagedList(page, pageSize, keyword);
            return PartialView("List", list);
        }


        // GET: Template/Edit/f25ef418-ecc6-11e4-91d9-6c92bf08396d?type=3&_=1430813183985
        [HttpGet]
        [AjaxOnly]
        public ActionResult Edit(string id, OpType type)
        {
            ViewBag.OpType = type;

            var types = _templateTypeManager.GetAll();
            List<SelectListItem> templateTypeitems = new List<SelectListItem>();

            if (types.Count > 0)
            {
                types.ForEach(t => templateTypeitems.Add(new SelectListItem()
                {
                    Text = t.Name,
                    Value = t.Id.ToString(),
                }));
            }
            ViewBag.TemplateTypes = templateTypeitems;

            switch (type)
            {
                case OpType.Update:
                case OpType.View:
                    var model = _manager.GetTvTemplateById(id);
                    return PartialView(model);
            }


            return PartialView();

        }
        // POST: Template/Edit/f25ef418-ecc6-11e4-91d9-6c92bf08396d
        [HttpPost]
        [AjaxOnly]
        public ActionResult Edit(OpType opType, TvTemplate dto)
        {
            var errorMsg = string.Empty;
            if (!string.IsNullOrEmpty(dto.Name))
            {
                switch (opType)
                {
                    case OpType.Add:
                        var exittemplate = _manager.GetElementByName(dto.Name);
                        if (exittemplate != null)
                        {
                            errorMsg = "模版名称已存在！";
                        }
                        else
                        {
                            dto.Id = Guid.NewGuid().ToString("N");
                            dto.CreateUser = this.CurrentUser.ChineseName;
                            dto.CreateDate = DateTime.Now;
                            dto.ModifyUser = this.CurrentUser.ChineseName;
                            dto.ModifyDate = DateTime.Now;
                            var newId = _manager.AddWithElements(dto);
                            _manager.SetTemplateRootElementCache(newId);
                        }
                        break;
                    case OpType.Update:
                        dto.ModifyUser = this.CurrentUser.ChineseName;
                        dto.ModifyDate = DateTime.Now;
                        _manager.Update(dto);
                        break;
                }
            }
            else
                errorMsg = "模板结构名称不能为空！";

            return Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
        }
        [HttpGet]
        [AjaxOnly]
        public ActionResult Preview(string address)
        {
            ViewBag.Address = address;
            return PartialView();

        }
        [AjaxOnly]
        public ActionResult BatchDelete(string id)
        {
            var errorMsg = string.Empty;
            var idArray = id.Split(',');
            if (_hotelManager.GetAllCoreSysHotels().Where(m => idArray.Contains(m.TemplateId) && !m.IsDelete).Count() == 0)
                _manager.BatchDelete(idArray);
            else
                errorMsg = "模板正在被使用中，不能删除！";
            return Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
        }

        #region Copy

        [HttpGet]
        [AjaxOnly]
        public ActionResult Copy(string id, OpType type)
        {
            ViewBag.OpType = type;
            switch (type)
            {
                case OpType.Copy:
                    var model = _manager.GetTvTemplateById(id);
                    return PartialView(new ViewCopyTemplate() { SourceTemplateId = model.Id, SourceTemplateName = model.Name, Name = "" });
            }
            return PartialView();
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult Copy(OpType opType, ViewCopyTemplate dto)
        {
            var errorMsg = string.Empty;
            if (string.IsNullOrEmpty(dto.Name))
            {
                errorMsg = "模板名称不能为空！";
            }

            var exittemplate = _manager.GetElementByName(dto.Name);
            if (exittemplate != null)
            {
                errorMsg = "模版名称已存在！";
            }

            if (string.IsNullOrWhiteSpace(errorMsg))
            {
                switch (opType)
                {
                    case OpType.Copy:
                        dto.CreateUser = base.CurrentUser.ChineseName;
                        var newId = _manager.Copy(dto);
                        _manager.SetTemplateRootElementCache(newId);
                        break;
                }
            }
            return Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
        }

        #endregion


        #region Elements

        [HttpGet]
        public ActionResult Elements(string id)
        {
            ViewBag.Template = id;
            return View();
        }

        [HttpGet]
        [AjaxOnly]
        public JsonResult GetElements(string id)
        {
            var result = _elementManager.GeElementNodes(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AjaxOnly]
        public JsonResult GeDocumentElements(string id)
        {
            var result = _elementManager.GeDocumentElementNodes(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除元算，级联删除下级元素及关联属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult ElementDelete(string id)
        {
            var errorMsg = string.Empty;
            var element = _elementManager.GetById(id);
            if (element.TemplateElement != null)
            {
                if (element.TemplateElement.Editable)
                    _elementManager.Delete(id);
                else
                    errorMsg = "当前节点不允许删除！";
            }
            return Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
        }



        public ActionResult EditElement(string id, OpType type)
        {

            if (id == null)
            {
                id = "";
            }
            var model = new TvDocumentElement();
            var parentName = "根目录";
            ViewBag.OpType = type;
            var types = _manager.GetAll();
            ViewBag.TemplateType = types.ToSelectListItems();

            var elementModel = _elementManager.GetOnlyElemnt(id);
            if (elementModel != null)
            {
                elementModel.TemplateElement = _templateElementManager.GetById(elementModel.TemplateElementId);
                elementModel.Attributes = _attributeManager.GetAllWithAttributes().Where(a => a.ElementId == elementModel.Id && string.IsNullOrEmpty(a.ParentId)).ToList();
            }
            if (elementModel != null)
            {
                switch (type)
                {
                    case OpType.Add:
                        model.ParentId = id;
                        model.TemplateId = elementModel.TemplateId;
                        break;
                    case OpType.Update:
                    case OpType.View:
                        model = elementModel;
                        break;
                }
            }
            if (model.ParentId != "" && model.ParentId != null)
            {
                var parentModel = _elementManager.GetOnlyElemnt(model.ParentId);
                parentName = parentModel.Name;
            }
            ViewBag.ParentName = parentName;
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult EditElement(OpType opType, TvDocumentElement dto)
        {
            string strResult = string.Empty;
            var element = _elementManager.GetById(dto.Id);
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                strResult = "节点名称不能为空！";
            }
            else if (_elementManager.DocumentNameIsExist(dto))
            {
                strResult = "节点名称已经存在！";
            }

            if (element.TemplateElement != null && string.IsNullOrWhiteSpace(strResult))
            {
                switch (opType)
                {
                    case OpType.Add:
                        if (element.TemplateElement.IsAllowChild)
                        {
                            var childFrame = _templateElementManager.GetAll().Where(e => e.ParentId == element.TemplateElement.Id && e.IsChildFrame).FirstOrDefault();
                            if (childFrame != null)
                            {
                                dto.Id = Guid.NewGuid().ToString("N");
                                dto.TemplateElementId = childFrame.Id;
                                dto.Id = _elementManager.Add(dto, childFrame.Attributes);
                                _elementManager.AddDocumentRootElementCache(dto.TemplateId, dto.Id);
                            }
                            else
                                strResult = "未找到子项框架！";
                        }
                        else
                            strResult = "此节点不允许添加子节点！";
                        break;
                    case OpType.Update:
                        if (element.TemplateElement.Editable)
                            _elementManager.Update(dto);
                        break;
                }
            }
            if (!string.IsNullOrWhiteSpace(strResult))
                return Json(new { errorMsg = strResult });
            else
            {
                var entity = _elementManager.GetById(dto.Id);
                return Json(entity);
            }
        }
        /// <summary>
        /// 根据模板ID查找模板类型
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        private TvTemplateType GetTemplateTypeByTemplateId(string templateId)
        {
            var template = _manager.GetTvTemplateById(templateId);
            return _templateTypeManager.GetById(template.TemplateTypeId);
        }

        #endregion


        #region Attrubite


        [HttpPost]
        public ActionResult EditAttribute(OpType opType, TvDocumentAttribute docAtt)
        {
            var errorMsg = string.Empty;
            try
            {
                switch (opType)
                {
                    case OpType.Add:
                        _attributeManager.Add(docAtt);
                        break;
                    case OpType.Update:
                        _attributeManager.Update(docAtt);
                        break;
                }

            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
        }

        #endregion


        //public ActionResult GetElementOfLisAttr(string elementId,string parrentId)
        //{
        //    TvDocumentElement obj = new TvDocumentElement();
        //    obj.Attributes = _attributeManager.GetAttributesByPrentId(elementId, parrentId);
        //    return PartialView(obj);
        //}



        public ActionResult GetElementOfLisAttr(string attributeId)
        {
            TvDocumentAttribute entity = _attributeManager.GetById(attributeId);
            if (entity != null)
            {
                List<TvDocumentAttributeData> list = entity.Value.JsonStringToObj<List<TvDocumentAttributeData>>();
                return PartialView(list);
            }
            return PartialView();
        }



        public ActionResult GetDocumentImgById(string Id)
        {
            var result = _attributeManager.GetAllWithAttributes().Where(p => p.Id == Id).Select(m => new
            {
                Id = m.Id,
                FilePath = m.Value
            }
            ).ToList();
            var json = result.ToJsonString();
            return Content(json);
        }


        public ActionResult AddRow()
        {
            return PartialView();
        }
        public ActionResult TemplatePreview(string id)
        {
            ViewBag.Title = "模板预览";
            object template = _elementManager.GetElementsByTemplateId(id).ToJsonString();

            return View(template);
        }

    }
}