

using System;
using System.Linq;
using System.Web.Mvc;
using YeahAppCentre.Web.Utility;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahCenter.Infrastructure;
using System.Collections.Generic;

namespace YeahAppCentre.Controllers
{

    public class TemplateTypeController : BaseController
    {

        private readonly ITvTemplateTypeManager _manager;
        private readonly ITvTemplateElementManager _elementManager;
        private readonly ITvTemplateAttributeManager _attributeManager;
        private readonly ITvTemplateManager _templateManager;

        public TemplateTypeController(
            ITvTemplateTypeManager manager,
            ITvTemplateElementManager elementManager,
            ITvTemplateAttributeManager attributeManager,
            ITvTemplateManager templateManager)
        {
            _manager = manager;
            _elementManager = elementManager;
            _attributeManager = attributeManager;
            _templateManager = templateManager;
        }

        #region TemplateType

        // GET: TemplateType
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

        // GET: TemplateType/Edit/5
        [HttpGet]
        [AjaxOnly]
        public ActionResult Edit(int id, OpType type)
        {
            ViewBag.OpType = type;
            switch (type)
            {
                case OpType.Update:
                case OpType.View:
                    var model = _manager.GetById(id);
                    return PartialView(model);
            }
            return PartialView();

        }

        // POST: TemplateType/Edit/5
        [HttpPost]
        [AjaxOnly]
        public ActionResult Edit(OpType opType, TvTemplateType dto)
        {
            var errorMsg = string.Empty;
            if (!string.IsNullOrEmpty(dto.Name))
            {
                switch (opType)
                {
                    case OpType.Add:
                        var exitTvTemplateType = _manager.GetByName(dto.Name);
                        if (exitTvTemplateType != null)
                        {
                            errorMsg = "模版结构已存在！";
                        }
                        else
                        {
                            _manager.AddWithBaseNode(dto);
                        }

                        break;
                    case OpType.Update:
                        _manager.Update(dto);
                        break;
                }
            }
            else
                errorMsg = "模板结构名称不能为空！";

            return Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
        }

        public ActionResult BatchDelete(string id)
        {
            var errorMsg = string.Empty;
            var types = id.Split(',').Select(m => m.ToInt()).ToArray();
            _manager.BatchDelete(types);
            return Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
        }
        /// <summary>
        /// 删除模板结构及子节点、属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult BatchDeleteWithChilds(string id)
        {
            var errorMsg = string.Empty;
            var types = id.Split(',').Select(m => m.ToInt()).ToArray();
            if (_templateManager.GetAll().Where(m => types.Contains(m.TemplateTypeId)).Count() == 0)
                _manager.BatchDeleteWithChilds(types);
            else
                errorMsg = "模板结构正在使用中，不能删除！";
            return Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
        }

        #endregion TemplateType

        #region Elements


        // GET: Role/Elements/5
        [HttpGet]
        public ActionResult Elements(int id)
        {
            ViewBag.TemplateType = id;
            return View();
        }

        [HttpGet]
        [AjaxOnly]
        public JsonResult GetElements(int id)
        {
            var result = _elementManager.GeElementNodes(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditElement(string id, OpType type)
        {
            if (id == null)
            {
                id = "";
            }
            var parentName = "根目录";

            ViewBag.OpType = type;

            var types = _manager.GetAll();

            ViewBag.TemplateType = types.ToSelectListItems();

            var model = new TvTemplateElement();
            switch (type)
            {
                case OpType.Add:
                    model.ParentId = id;
                    break;
                case OpType.Update:
                case OpType.View:
                    model = _elementManager.GetById(id);
                    break;

            }
            if (model.ParentId != "" && model.ParentId != null)
            {
                var parentModel = _elementManager.GetById(model.ParentId);
                parentName = parentModel.Name;
            }
            ViewBag.ParentName = parentName;
            return PartialView(model);
        }

        // POST: Role/EidtElement/5
        [HttpPost]
        public ActionResult EditElement(OpType opType, TvTemplateElement dto)
        {
            string strResult = string.Empty;
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                strResult = "节点名称不能为空！";
            }
            else if (_elementManager.ElementNameIsExist(dto))
            {
                strResult = "节点名称已经存在！";
            }

            if (string.IsNullOrWhiteSpace(strResult))
            {
                switch (opType)
                {
                    case OpType.Add:
                        dto.Id = _elementManager.Add(dto);
                        break;
                    case OpType.Update:
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

        [HttpPost]
        [AjaxOnly]
        public ActionResult ElementDelete(string id)
        {
            var errorMsg = string.Empty;
            _elementManager.Delete(id);
            return Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
        }

        /// <summary>
        /// 删除元算，级联删除下级元素及关联属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult ElementDeleteWithChilds(string id)
        {
            var errorMsg = string.Empty;
            var element = _elementManager.GetById(id);
            _elementManager.DeleteWithChilds(id);
            return Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
        }


        #endregion Elements

        #region Attribute


        [HttpPost]
        public ActionResult EditAttribute(OpType opType, TvTemplateAttribute dto)
        {
            var errorMsg = string.Empty;
            try
            {
                switch (opType)
                {
                    case OpType.Add:
                        _attributeManager.Add(dto);
                        break;
                    case OpType.Update:
                        _attributeManager.Update(dto);
                        break;
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
        }



        [HttpPost]
        [AjaxOnly]
        public ActionResult AttributeDelete(string id)
        {
            var errorMsg = string.Empty;
            _attributeManager.Delete(id);
            return Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
        }


        #endregion Attribute
        /// <summary>
        /// list类型的子属性
        /// </summary>
        /// <param name="parrentId"></param>
        /// <returns></returns>
      /*  public ActionResult  GetElementOfLisAttr(string elementId,string parrentId)
        {
            TvTemplateElement obj = new TvTemplateElement();
            obj.Attributes = _attributeManager.GetAttributesByPrentId(elementId,parrentId);
            ViewBag.Status = "1";
            if (obj.Attributes.Count==0)
            {
                ViewBag.Status = "0";
                ViewBag.ParrentId = parrentId;
            }
            return PartialView(obj);
        }*/


        public ActionResult GetElementOfLisAttr(string attributeId)
        {
            TvTemplateAttribute entity = _attributeManager.GetById(attributeId);
            if (entity != null)
            {
                List<TvTemplateAttribute> list = entity.Value.JsonStringToObj<List<TvTemplateAttribute>>();
                return PartialView(list);
            }
            return PartialView();
        }

        public ActionResult AddRow()
        {
            return PartialView();
        }
    }
}
