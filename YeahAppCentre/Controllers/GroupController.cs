using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class GroupController : Controller
    {
        IGroupManager groupManager;
        ILogManager logManager;
        // GET: HotelGroup

        public GroupController(IGroupManager groupManager,
        ILogManager logManager)
        {
            this.groupManager = groupManager;
            this.logManager = logManager;
        }
        public ActionResult Index(GroupCriteria groupCriteria)
        {
            groupCriteria.NeedPaging = true;

            var partialViewResult = this.List(groupCriteria) as PartialViewResult;
            if (partialViewResult != null)
                this.ViewBag.List = partialViewResult.Model;

            ViewBag.GroupCriteria = groupCriteria;

            return View();
        }

        [AjaxOnly]
        public ActionResult List(GroupCriteria groupCriteria)
        {
            var list = new PagedViewList<CoreSysGroup>();
            groupCriteria.PageSize = 10;
            groupCriteria.NeedPaging = true;
            list.PageIndex = groupCriteria.Page;
            list.PageSize = groupCriteria.PageSize;
            list.Source = groupManager.Search(groupCriteria);
            list.TotalCount = groupCriteria.TotalCount;
            return this.PartialView("List", list);
        }

        public ActionResult AddOrEdit(string id, OpType type)
        {
            CoreSysGroup group = new CoreSysGroup();
            ViewBag.OpType = type;
            switch (type)
            {
                case OpType.Add:
                   var  obj = groupManager.GetAll().OrderByDescending(m => m.GroupCode).FirstOrDefault();
                    if (obj != null)
                    {
                        group.GroupCode = int.Parse((int.Parse(obj.GroupCode) + 1).ToString()).ToString("000");
                    }
                    break;
                case OpType.View:
                case OpType.Update:
                    group = groupManager.GetGroup(id);
                    break;
                default:
                    break;
            }

            return PartialView(group);
        }
        [HttpPost]
        public ActionResult AddOrEdit(CoreSysGroup groupObj, OpType type)
        {
            string errorMsg = string.Empty;
            try
            {
                switch (type)
                {
                    case OpType.Add:
                        groupManager.Insert(groupObj);
                        break;
                    case OpType.Update:
                        groupManager.Update(groupObj);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return this.Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
        }
    }
}