using System;
using System.Web.Mvc;
using YeahAppCentre.Web.Utility;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;
using System.Linq;
using System.Collections.Generic;
using YeahCenter.Infrastructure;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;

namespace YeahAppCentre.Controllers
{
    public class SystemLogController : Controller
    {
        private ISystemLogManager systemManager;
        private ILogManager logManager;
        private IMongoLogManager mongoLogManager;

        public SystemLogController(ISystemLogManager iSystemManager, ILogManager logManager,IMongoLogManager mongoLogManager)
        {
            this.systemManager = iSystemManager;
            this.logManager = logManager;
            this.mongoLogManager = mongoLogManager;
           
        }
        // GET: SystemLog
        public ActionResult Index(MongoCriteria logCriteria = null)
        {
            if (logCriteria == null)
            {
                logCriteria = new MongoCriteria();
            }

            var logTypeList = YeahAppCentre.Web.Utility.DropDownExtensions.GetSelectList(typeof(LogType)).ToList();
            logTypeList.Remove(logTypeList.FirstOrDefault(type => type.Value == LogType.UserBehavior.GetValueStr()));
            ViewBag.LogType = logTypeList;

            var partialViewResult = this.List(logCriteria) as PartialViewResult;
            if (partialViewResult != null)
                this.ViewBag.List = partialViewResult.Model;
            ViewBag.LogCriteria = logCriteria;

            return View();
        }
        [AjaxOnly]
        public ActionResult List(MongoCriteria logCriteria)
        {

            if (logCriteria == null)
                logCriteria = new MongoCriteria();
            var list = new PagedViewList<MongoLog>();

            logCriteria.SortFiled = "CreateTime";
            logCriteria.OrderAsc = false;
            logCriteria.NeedPaging = true;
            
            if (!string.IsNullOrWhiteSpace(logCriteria.LogType.ToString()))
                logCriteria.LogType = (LogType)Enum.Parse(typeof(LogType), logCriteria.LogType.GetValueStr());
            if (!string.IsNullOrWhiteSpace(logCriteria.AppType.ToString()))
                logCriteria.AppType = (AppType)Enum.Parse(typeof(AppType), logCriteria.AppType.GetValueStr());
            list.PageIndex = logCriteria.Page;
            list.PageSize = logCriteria.PageSize;
            list.Source = mongoLogManager.Search(logCriteria);
            list.TotalCount = logCriteria.TotalCount;
         

            return this.PartialView("List", list);
        }
        [HttpGet]
        [AjaxOnly]
        public ActionResult Edit(string id, OpType type)
        {
            ViewBag.OpType = type;
            return PartialView(mongoLogManager.GetById(id));
        }
    }
}