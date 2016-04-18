using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YeahAppCentre.Web.Utility;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahCenter.Infrastructure;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.DomainModel.Enum;

namespace YeahAppCentre.Controllers
{
    public class BehaviorLogController : BaseController
    {
        private readonly IBehaviorLogManager _manager;
        public BehaviorLogController(
            IBehaviorLogManager manager,
            ILogManager logmanager,
            IHttpContextService httpContextService)
            : base(logmanager, httpContextService)
        {
            _manager = manager;
        }
        // GET: BehaviorLog
        public ActionResult Index(LogCriteria logCriteria = null)
        {
            if (logCriteria == null) 
                logCriteria = new LogCriteria();

            logCriteria.NeedPaging = true;

            var partialViewResult = List(logCriteria) as PartialViewResult;
            if (partialViewResult != null)
                ViewBag.List = partialViewResult.Model;
            ViewBag.LogCriteria = logCriteria;

            return View();
        }

        [AjaxOnly]
        public ActionResult List(LogCriteria logCriteria)
        {
            if (logCriteria == null)
                logCriteria = new LogCriteria();

            var list = new PagedViewList<BehaviorLog>();
            logCriteria.SortFiled = "CreateTime";
            logCriteria.OrderAsc = false;
            logCriteria.NeedPaging = true;
            list.PageIndex = logCriteria.Page;
            list.PageSize = logCriteria.PageSize;
            list.Source = _manager.Search(logCriteria);
            list.TotalCount = logCriteria.TotalCount;

            return this.PartialView("List", list);
        }
        [HttpGet]
        [AjaxOnly]
        public ActionResult Edit(string id, OpType type)
        {
            ViewBag.OpType = type;
            return PartialView(_manager.GetById(id));
        }
    }
}