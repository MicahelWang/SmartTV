using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YeahAppCentre.Web.Utility;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;

namespace YeahAppCentre.Controllers
{
    public class AppPublishController : BaseController
    {
        private IAppLibraryManager appLibraryManager;
        private ILogManager logManager;
        private ITVHotelConfigManager tVHotelConfigManager;
        private IDeviceTraceLibraryManager deviceManager;
        public AppPublishController(IAppLibraryManager appLibraryManager, ILogManager logManager, IDeviceTraceLibraryManager deviceManager)
        {
            this.appLibraryManager = appLibraryManager;
            this.logManager = logManager;
            this.deviceManager = deviceManager;
        }

        // GET: AppPublish
        public ActionResult Index(AppPublishCriteria appPublishCriteria = null)
        {
            if (appPublishCriteria == null)
            {
                appPublishCriteria = new AppPublishCriteria();
            }
            appPublishCriteria.NeedPaging = true;

            var partialViewResult = this.List(appPublishCriteria) as PartialViewResult;
            if (partialViewResult != null)
                this.ViewBag.List = partialViewResult.Model;
            ViewBag.appPublishCriteria = appPublishCriteria;
            ViewBag.keyWord = "";

            if (!string.IsNullOrEmpty(appPublishCriteria.AppId))
            {
                var Appname = appLibraryManager.GetAppByAppId(appPublishCriteria.AppId).Name;

                ViewBag.Appname = Appname;
            }
            return View();
        }

        [AjaxOnly]
        public ActionResult Edit(string id, string version, string hotelId, OpType type)
        {
            int intVersion;
            int.TryParse(version, out intVersion);
            ViewBag.OpType = type;
            ViewBag.DefaultSelect = new List<SelectListItem>() { DropDownExtensions.DefaultItem };
            switch (type)
            {
                case OpType.View:
                    var model = appLibraryManager.SearchAppPublishs(new AppPublishCriteria { AppId = id, VersionCode = intVersion, HotelId = hotelId })
                        .Find(m => m.HotelId == hotelId);
                    return PartialView(model);
                case OpType.Add:
                    var apps = appLibraryManager.Search(new AppsCriteria { Active = true, AppVersionActive = true });
                    if (apps != null)
                    {
                        var appsList = DropDownExtensions.ToSelectListItems(apps);
                        appsList.ToList().Insert(0, new SelectListItem() { Text = "-请选择-", Value = "" });
                        ViewBag.Apps = appsList;
                        return PartialView();
                    }
                    else
                    {
                        return this.Content("所填酒店没可绑定的设备");
                    }
                case OpType.Update:
                    apps = appLibraryManager.Search(new AppsCriteria { Active = true, AppVersionActive = true });
                    var appList = apps.Select(a => new SelectListItem
                        {
                            Text = a.Name,
                            Value = a.Id
                        });

                    ViewBag.Apps = appList;
                    model = appLibraryManager.SearchAppPublishs(new AppPublishCriteria { AppId = id, VersionCode = intVersion, HotelId = hotelId })
                        .Find(m=>m.HotelId==hotelId);
                    return PartialView(model);
            }
            return PartialView();

        }


        [HttpPost]
        [AjaxOnly]
        public ActionResult Edit(OpType opType, AppPublish dto)
        {
            //Tuple<List<string>, List<AppPublish>> deviceList = null;
            //if (dto.BindType == BindingType.Hotel)
            //{
            //    if (!string.IsNullOrWhiteSpace(dto.HotelId))
            //    {
            //        deviceList = appLibraryManager.DeviceSeriesFilter(dto.Id, dto.VersionCode, dto.HotelId);
            //        if (deviceList.Item1.Count == 0 && deviceList.Item2.Count == 0)
            //        {
            //            return Content("该酒店没有任何设备");
            //        }
            //    }

            //    else
            //    {
            //        return Content("酒店名称不能为空");
            //    }
            //}


            return base.ExecutionMethod(() =>
        {
            var errorMsg = string.Empty;
            string currentUser = CurrentUser.ChineseName;
            dto.LastUpdateTime = DateTime.Now;
            switch (opType)
            {
                case OpType.Add:
                    errorMsg = InsertOrUpData(dto, currentUser);
                    break;
                case OpType.Update:
                    dto.LastUpdater = currentUser;
                    dto.LastUpdateTime = DateTime.Now;

                    appLibraryManager.UpdateAppPublish(dto);
                    break;
            }

            return this.Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
        });

        }
        private string InsertOrUpData(AppPublish dto, string currentUser)
        {

            try
            {
                return AddAppVersionPublish(new AppPublish()
                      {
                          Id = dto.Id,
                          VersionCode = dto.VersionCode,
                          Active = dto.Active,
                          PublishDate = dto.PublishDate,
                          CreateTime = DateTime.Now,
                          LastUpdater = currentUser,
                          LastUpdateTime = DateTime.Now,
                          HotelId = dto.HotelId
                      }, currentUser);

            }
            catch (Exception e)
            {
                throw new Exception("插入数据操作失败" + e.Message);
            }

            //foreach (AppPublish app in appPub.Item2)
            //{
            //    try
            //    {
            //        app.PublishDate = dto.PublishDate;
            //        app.Active = dto.Active;
            //        app.LastUpdateTime = DateTime.Now;
            //        app.LastUpdater = currentUser;
            //        appLibraryManager.UpdateAppPublishWithOutCopyTo(app);

            //    }
            //    catch (Exception e)
            //    {
            //        throw new Exception("更新数据失败" + e.Message);
            //    }

            //}
            //return "Success";
        }

        private string AddAppVersionPublish(AppPublish dto, string currentUser)
        {
            var exitAppPublish = appLibraryManager.SearchAppPublishs(new AppPublishCriteria { AppId = dto.Id });

            if (exitAppPublish != null && exitAppPublish.Any(e => e.VersionCode.Equals(dto.VersionCode) && e.HotelId.Equals(dto.HotelId)))
            {
                return "该记录已经存在！";
            }
            else
            {
                //var appVersion = appLibraryManager.SearchAppVersions(new AppsCriteria { Id = dto.Id });

                dto.LastUpdater = currentUser;
                dto.CreateTime = DateTime.Now;
                dto.LastUpdateTime = DateTime.Now;
            }

            appLibraryManager.AddPublish(dto);

            return "保存成功";
        }

        [AjaxOnly]
        public ActionResult List(AppPublishCriteria appPublishCriteria)
        {
            var list = new PagedViewList<AppPublish>();

            appPublishCriteria.NeedPaging = true;
            list.PageIndex = appPublishCriteria.Page;
            list.PageSize = appPublishCriteria.PageSize;
            list.Source = appLibraryManager.SearchAppPublishs(appPublishCriteria);
            list.TotalCount = appPublishCriteria.TotalCount;

            return this.PartialView("List", list);
        }


        [HttpGet]
        [AjaxOnly]
        public JsonResult GetVersionByAppId(string id)
        {
            var result = appLibraryManager.SearchAppVersions(new AppsCriteria { Id = id });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        public ActionResult AddAppPublish(string id, string version, string serisecode)
        {

            ViewBag.DefaultSelect = new List<SelectListItem>() { DropDownExtensions.DefaultItem };

            var apps = appLibraryManager.Search(new AppsCriteria { Active = true });
            var appList = apps.Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id
                });
            appList.ToList().Insert(0, new SelectListItem() { Text = "-请选择-", Value = "" });
            ViewBag.Apps = appList;

            return PartialView();
        }
        [HttpPost]
        public ActionResult AddAppPublish(ViewAppPublish appPub)
        {

            ViewBag.DefaultSelect = new List<SelectListItem>() { DropDownExtensions.DefaultItem };

            return this.Content(string.IsNullOrEmpty(null) ? "Success" : null);
        }


    }
}