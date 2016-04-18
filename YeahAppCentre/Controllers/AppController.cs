namespace YeahAppCentre.Controllers
{
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

    public class AppController : BaseController
    {
        private IAppLibraryManager appLibraryManager;
        private ILogManager logManager;

        public AppController(
            IAppLibraryManager appLibraryManager,
            ILogManager logManager,
            IHttpContextService httpContextService)
            : base(logManager, httpContextService)
        {
            this.appLibraryManager = appLibraryManager;
            this.logManager = logManager;
        }

        // GET: APP
        public ActionResult Index(AppsCriteria appsCriteria)
        { 
            appsCriteria.NeedVersion = true;
            appsCriteria.NeedPaging = true;

            var partialViewResult = this.List(appsCriteria) as PartialViewResult;
            if (partialViewResult != null)
                this.ViewBag.List = partialViewResult.Model;

            ViewBag.AppsCriteria = appsCriteria;

            return View();
        }

        [HttpGet]
        [AjaxOnly]
        public ActionResult Edit(string id,string version, OpType type)
        {
            ViewBag.OpType = type;

            switch (type)
            {
                case OpType.Update:
                case OpType.View:
                    int intVersion;
                    int.TryParse(version, out intVersion);
                    var model = appLibraryManager.SearchAppVersions(new AppsCriteria { Id = id, AppVersion = intVersion }).FirstOrDefault();
                    return PartialView(model);
            }
            return PartialView();
        }

        [HttpGet]
        [AjaxOnly]
        public ActionResult EditApp(string id, OpType type)
        {
            ViewBag.OpType = type;

            switch (type)
            {
                case OpType.Update:
                case OpType.View:
                    var model = appLibraryManager.GetAppByAppId(id);
                    return PartialView(model);
            }
            return PartialView();
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult EditApp(OpType opType, Apps dto)
        {
            return base.ExecutionMethod(() =>
            {
                var errorMsg = string.Empty;
                string currentUser = CurrentUser.ChineseName;

                switch (opType)
                {
                    case OpType.Update:

                        dto.LastUpdater = currentUser;
                        dto.LastUpdateTime = DateTime.Now;

                        appLibraryManager.Update(dto);
                        break;
                }

                return this.Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
            });
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult Edit(OpType opType, AppVersion dto)
        {
            return base.ExecutionMethod(() => 
            {
                var errorMsg = string.Empty;
                string currentUser = CurrentUser.ChineseName;
                dto.LastUpdateTime = DateTime.Now;

                switch (opType)
                {
                    case OpType.Add:
                        errorMsg = AddAppVersion(dto, currentUser);
                        break;
                    case OpType.Update:
                        dto.LastUpdater = currentUser;
                        dto.LastUpdateTime = DateTime.Now;

                        dto.App.LastUpdater = dto.LastUpdater;
                        dto.App.LastUpdateTime = dto.LastUpdateTime;

                        appLibraryManager.UpdateAppVersion(dto);
                        break;
                }

                return this.Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
            });
        }

        private string AddAppVersion(AppVersion dto, string currentUser)
        {
            var exitAppversions = appLibraryManager.SearchAppVersions(new AppsCriteria { AppName = dto.App.Name });

            if (exitAppversions != null && exitAppversions.Any(e => e.VersionCode.Equals(dto.VersionCode)))
            {
                return "该APP此版本已经存在！";
            }
            else if (exitAppversions != null && exitAppversions.Any())
            {
                dto.LastUpdater = currentUser;
                dto.Id = exitAppversions.FirstOrDefault().Id;

                dto.CreateTime = DateTime.Now;
                dto.LastUpdateTime = DateTime.Now;
                dto.App = null;
            }
            else
            {
                dto.LastUpdater = currentUser;
                dto.Id = Guid.NewGuid().ToString();
                dto.CreateTime = DateTime.Now;
                dto.LastUpdateTime = DateTime.Now;

                dto.App.Id = dto.Id;
                dto.App.LastUpdater = dto.LastUpdater;
                dto.App.CreateTime = dto.CreateTime;
                dto.App.LastUpdateTime = dto.LastUpdateTime;
                dto.App.SecureKey = Guid.NewGuid().ToString();
                dto.App.AppKey = Guid.NewGuid().ToString();
                dto.App.Platfrom = "Android";
                dto.App.Active = dto.Active;

            }
            appLibraryManager.AddVersion(dto);

            return string.Empty;
        }

        [AjaxOnly]
        public ActionResult List(AppsCriteria appsCriteria)
        {
            var list = new PagedViewList<Apps>();
            appsCriteria.NeedPaging = true;
            appsCriteria.PageSize = 10;

            list.PageIndex = appsCriteria.Page;
            list.PageSize = appsCriteria.PageSize;
            list.Source = appLibraryManager.Search(appsCriteria);
            list.TotalCount = appsCriteria.TotalCount;

            return this.PartialView("List", list);
        }
        [AjaxOnly]
        public ActionResult AppVersionList(string appId)
        {
            var list = new PagedViewList<AppVersion>();
            var appsCriteria = new AppsCriteria();
            appsCriteria.Id = appId;
            appsCriteria.NeedPaging = true;
            list.PageIndex = appsCriteria.Page;
            list.PageSize = appsCriteria.PageSize;
            list.Source = appLibraryManager.SearchAppVersions(appsCriteria);
            list.TotalCount = appsCriteria.TotalCount;

            return this.PartialView("AppVersionList", list);
        }

        [AjaxOnly]
        public JsonResult GetAppNamebyPageName(string packagename)
        {
            var result = appLibraryManager.SearchAppsFromCache(null, null, packagename).FirstOrDefault();
            return Json(result == null ? "" : result.Name, JsonRequestBehavior.AllowGet);
        }
    }
}