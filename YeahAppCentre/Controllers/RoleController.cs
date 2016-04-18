using System;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using Newtonsoft.Json;
using YeahAppCentre.Web.Utility;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahCenter.Infrastructure;

namespace YeahAppCentre.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleManager _manager;

        public RoleController(IRoleManager manager)
        {
            this._manager = manager;
        }

        // GET: Role
        public ActionResult Index()
        {
            var partialViewResult = this.List() as PartialViewResult;
            if (partialViewResult != null)
                this.ViewBag.List = partialViewResult.Model;
            ViewBag.keyWord = "";
            return View();
        }

      

        // GET: Role/Edit/5
        [HttpGet]
        [AjaxOnly]
        public ActionResult Edit(string id, OpType type)
        {
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
        public ActionResult Edit(OpType opType, ErpPowerRole dto)
        {
            var errorMsg = string.Empty;
            switch (opType)
            {
                case OpType.Add:
                    var exitErpPowerRole = _manager.GetEntityName(dto.RoleName);
                    if (exitErpPowerRole !=null)
                    {
                        errorMsg = "该角色已存在";
                    }
                    else 
                    {
                        _manager.Add(dto);
                    }
                   
                      
                    
                    break;
                case OpType.Update:
                    _manager.Update(dto);
                    break;
            }
           
            return this.Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
        }
        [AjaxOnly]
        public ActionResult List(string keyword = "", int page = 0)
        {
            const int pageSize = 10;
            var list = this._manager.PagedList(page, pageSize, keyword);
            return this.PartialView("List", list);
        }
        [HttpPost]
        [AjaxOnly]
        public ActionResult BatchDelete(string id)
        {
            var errorMsg = string.Empty;
            var roles = id.Split(',');
            _manager.BatchDelete(roles);
            return this.Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
        }

        [HttpGet]
        public ActionResult EditPower(string id)
        {
            ViewBag.RoleId = id;
            var resourceIds = _manager.GetResourcesIdByRole(id);
            ViewBag.ResourceIds=string.Join(",",resourceIds);
            return PartialView();
        }

        [HttpPost]
        public ActionResult EditPower(string id,string funcList)
        {
             string errorMsg = string.Empty;
            ViewBag.RoleId = id;
            int[] Ids = funcList.Split(',').Select(m => m.ToInt()).ToArray();
            _manager.UpdatePower(id, Ids);
            return this.Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
        }

        public string GetPowersByRole(string id)
        {
            var treeNodes = _manager.GetPower(id);
            var zNode = JsonConvert.SerializeObject(treeNodes).Replace("ischecked", "checked");
            return zNode;
        }
    }
}
