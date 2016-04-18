namespace YeahAppCentre.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using YeahAppCentre.Web.Utility;
    using YeahCenter.Infrastructure;
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Enum;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApiLibrary.Infrastructure;

    public class DeviceTraceController : BaseController
    {
        private IDeviceTraceLibraryManager traceManager;
        private IBackupDeviceManager backupManager;
        private IAppLibraryManager appManager;
        private IHotelManager hotelManager;
        private IAppLibraryManager appLibraryManager;
        private IHotelMovieTraceNoTemplateWrapperFacade hotelMovieTraceNoTemplateWrapperFacade;

        public DeviceTraceController(IDeviceTraceLibraryManager traceManager,
            IAppLibraryManager appManager,
            ILogManager logManager,
            IHotelManager hotelManager,
            IAppLibraryManager appLibraryManager,
            IBackupDeviceManager backupManager,
            IHttpContextService httpContextService,
            IHotelMovieTraceNoTemplateWrapperFacade hotelMovieTraceNoTemplateWrapperFacade)
            : base(logManager, httpContextService)
        {
            this.traceManager = traceManager;
            this.appManager = appManager;
            this.hotelManager = hotelManager;
            this.appLibraryManager = appLibraryManager;
            this.backupManager = backupManager;
            this.hotelMovieTraceNoTemplateWrapperFacade = hotelMovieTraceNoTemplateWrapperFacade;
        }

        // GET: APP
        public ActionResult Index(TraceCriteria traceCriteria)
        {

            traceCriteria.NeedPaging = true;

            var partialViewResult = this.List(traceCriteria) as PartialViewResult;
            if (partialViewResult != null)
                this.ViewBag.List = partialViewResult.Model;

            ViewBag.TraceCriteria = traceCriteria;

            if (!string.IsNullOrEmpty(traceCriteria.HotelId))
            {
                var hotelName = hotelManager.GetHotel(traceCriteria.HotelId).HotelName;

                var hotelcode = hotelManager.GetHotel(traceCriteria.HotelId).HotelCode;
                ViewBag.HotelName = hotelName + "(" + hotelcode + ")";
            }


            return View();
        }

        [HttpGet]
        [AjaxOnly]
        public ActionResult AddOrEdite(string serisecode, OpType type)
        {
            Dictionary<string, string> dicType = new Dictionary<string, string>() 
            { 
                {DeviceType.AIO.ToString(),DeviceType.AIO.GetDescription()},{DeviceType.STB.ToString(),DeviceType.STB.GetDescription()}
                ,{DeviceType.HCSServer.ToString(),DeviceType.HCSServer.GetDescription()}
            };

            List<SelectListItem> modelTypeList = new List<SelectListItem>();

            var allHotelList = hotelManager.GetAllHotels().ToList();
            foreach (var item in dicType)
            {
                modelTypeList.Add(new SelectListItem() { Value = item.Key.ToString(), Text = item.Value });
            }
            //modelTypeList.Insert(0, new SelectListItem() { Value = "", Text = "==请选择==" });
            ViewBag.selectList = modelTypeList;



            var apps = appLibraryManager.Search(new AppsCriteria { Active = true });



            ViewBag.DefaultSelect = new List<SelectListItem>() { DropDownExtensions.DefaultItem };
            ViewBag.AppIds = apps.ToSelectListItems();

            ViewBag.OpType = type;

            switch (type)
            {
                case OpType.Update:
                case OpType.View:
                    var model = traceManager.Search(new DeviceTraceCriteria { DeviceSeries = serisecode }).FirstOrDefault();
                    ViewBag.HotelName = string.IsNullOrWhiteSpace(model.HotelId) ? "" : GetHotelNameById(model.HotelId);

                    var removeList = new List<SelectListItem>();
                    modelTypeList.ForEach(m =>
                    {
                        if (model.DeviceType.ParseAsEnum<DeviceType>() == DeviceType.HCSServer ? m.Value != DeviceType.HCSServer.ToString() : m.Value == DeviceType.HCSServer.ToString())
                        {
                            removeList.Add(m);
                        }
                    });
                    removeList.ForEach(m => modelTypeList.Remove(m));

                    return PartialView(model);
            }
            return PartialView(new DeviceTrace());
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult AddOrEdite(OpType opType, DeviceTrace dto)
        {
            return base.ExecutionMethod(() =>
            {
                var errorMsg = string.Empty;
                dto.LastVisitTime = DateTime.Now;

                switch (opType)
                {
                    case OpType.Add:
                        errorMsg = AddDeviceTrace(dto);

                        try
                        {
                            if (string.IsNullOrWhiteSpace(errorMsg))
                                hotelMovieTraceNoTemplateWrapperFacade.DistributeByDevice(dto);
                        }
                        catch (Exception ex)
                        {
                            errorMsg = string.Format("电影分发异常：{0}",ex.Message);
                        }

                        break;
                    case OpType.Update:
                        dto.LastVisitTime = DateTime.Now;
                        dto.GroupId = hotelManager.GetHotel(dto.HotelId).GroupId;
                        traceManager.Update(dto);
                        break;
                }

                return this.Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
            });
        }

        private string AddDeviceTrace(DeviceTrace dto)
        {
            var exitAppversions = traceManager.Search(new DeviceTraceCriteria { DeviceSeries = dto.DeviceSeries });

            if (exitAppversions != null && exitAppversions.Any())
            {
                return "该绑定记录已经存在！";
            }
            else
            {
                dto.DeviceKey = SecurityManager.GetRNGString(30, 2);
                dto.FirstVisitTime = DateTime.Now;
                dto.GroupId = hotelManager.GetHotel(dto.HotelId).GroupId;
                dto.LastVisitTime = DateTime.Now;

            }
            traceManager.Add(dto);

            return string.Empty;
        }

        [AjaxOnly]
        public ActionResult List(TraceCriteria traceCriteria)
        {
            var list = new PagedViewList<DeviceTrace>();

            if (traceCriteria.SortFiled.Equals("Id"))
            {
                traceCriteria.SortFiled = "LastVisitTime";
                traceCriteria.OrderAsc = false;
            }
            traceCriteria.NeedPaging = true;
            list.PageIndex = traceCriteria.Page;
            list.PageSize = traceCriteria.PageSize;
            list.Source = traceManager.Search(traceCriteria);
            list.TotalCount = traceCriteria.TotalCount;

            return this.PartialView("List", list);
        }

        public string GetHotelNameById(string hotelId)
        {
            var result = string.Empty;
            var hotelEntity = hotelManager.GetCoreSysHotelById(hotelId);
            if (hotelEntity != null)
            {
                result = hotelEntity.HotelName;
            }
            return result;
        }

        public string GetGroupNameById(string hotelId)
        {
            return hotelManager.GetHotelObject(hotelId).Group.GroupName;
        }

        public string GetAppNameById(string appId)
        {
            var app = appManager.SearchAppsFromCache(null, null).FirstOrDefault(f => f.Id.Equals(appId));
            return app == null ? appId : app.Name;
        }



        [HttpGet]
        [AjaxOnly]
        public JsonResult GetAppversionByAppId(string id)
        {
            var result = appLibraryManager.SearchAppVersions(new AppsCriteria { Id = id });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ChangeToBackUp(DeviceTrace dto)
        {

            BackupDevice backup = backupManager.Search(new BackupDeviceCriteria() { DeviceSeries = dto.DeviceSeries }).FirstOrDefault();
            DeviceTrace device = traceManager.Search(new TraceCriteria() { DeviceSeries = dto.DeviceSeries }).FirstOrDefault();
            return base.ExecutionMethod(() =>
            {
                if (device == null)
                {
                    return Json("找不到该设备！");
                }
                if (backup == null)
                {
                    backup = new BackupDevice()
                    {
                        DeviceSeries = device.DeviceSeries,
                        DeviceType = device.DeviceType,
                        HotelId = device.HotelId,
                        Model = string.IsNullOrEmpty(device.Model) == true ? "" : device.Model,
                        LastUpdateTime = DateTime.Now,
                        Active = false,
                        IsUsed = false,
                        LastUpdatUser = base.CurrentUser.Account
                    };
                    backupManager.Add(backup);
                }
                else
                {
                    backup.IsUsed = false;
                    backup.Active = false;
                    backup.LastUpdateTime = DateTime.Now;
                    backup.HotelId = device.HotelId;
                    backup.Model = string.IsNullOrEmpty(device.Model) == true ? "" : device.Model;
                    backup.LastUpdatUser = base.CurrentUser.Account;
                    backupManager.Update(backup);
                }
                traceManager.Delete(device);
                return Json("Success");
            }, false);
        }
    }
}