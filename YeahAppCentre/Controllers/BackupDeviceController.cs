using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;
using System.Web.Helpers;
using YeahAppCentre.Web.Utility;
using YeahCenter.Infrastructure;

namespace YeahAppCentre.Controllers
{
    public class BackupDeviceController : BaseController
    {
        private IBackupDeviceManager backupdevicemanager;
        private IDeviceTraceLibraryManager deviceTrancemanager;
        private ILogManager logManager;
        private IHotelManager hotelManager;

        public BackupDeviceController(
            IBackupDeviceManager backupdevicemanager,
            ILogManager logManager,
              IHotelManager hotelManager,
            IHttpContextService httpContextService, IDeviceTraceLibraryManager deviceTrancemanager)
            : base(logManager, httpContextService)
        {
            this.backupdevicemanager = backupdevicemanager;
            this.logManager = logManager;
            this.hotelManager = hotelManager;
            this.deviceTrancemanager = deviceTrancemanager;
        }

        // GET: BackupDevice
        public ActionResult Index(BackupDeviceCriteria backupDeviceCriteria = null)
        {
            if (backupDeviceCriteria == null)
            {
                backupDeviceCriteria = new BackupDeviceCriteria();
            }
            var partialViewResult = this.List(backupDeviceCriteria) as PartialViewResult;
            if (partialViewResult != null)
                this.ViewBag.List = partialViewResult.Model;
            ViewBag.backupdevicecriteria = backupDeviceCriteria;
            if (!string.IsNullOrEmpty(backupDeviceCriteria.HotelId))
            {
                var hotelname = hotelManager.GetHotel(backupDeviceCriteria.HotelId).HotelName;

                var hotelcode = hotelManager.GetHotel(backupDeviceCriteria.HotelId).HotelCode;


                ViewBag.HotelName = hotelname + "(" + hotelcode + ")";


            }
            return View();
        }

        public ActionResult List(BackupDeviceCriteria backupDeviceCriteria)
        {
            var list = new PagedViewList<BackupDevice>();

            backupDeviceCriteria.NeedPaging = true;
            list.PageIndex = backupDeviceCriteria.Page;
            list.PageSize = backupDeviceCriteria.PageSize;
            list.Source = backupdevicemanager.Search(backupDeviceCriteria);
            list.TotalCount = backupDeviceCriteria.TotalCount;

            return this.PartialView("List", list);
        }

        [HttpGet]
        [AjaxOnly]
        public ActionResult AddOrEdit(int id, OpType type)
        {
            // ViewBag.DefaultSelect = new List<SelectListItem>() { DropDownExtensions.DefaultItem };

            Dictionary<string, string> dicType = new Dictionary<string, string>() 
            { 
                {DeviceType.AIO.ToString(),DeviceType.AIO.GetDescription()},{DeviceType.STB.ToString(),DeviceType.STB.GetDescription()}
            };

            List<SelectListItem> modelTypeList = new List<SelectListItem>();

            var allHotelList = hotelManager.GetAllHotels().ToList();
            foreach (var item in dicType)
            {
                modelTypeList.Add(new SelectListItem() { Value = item.Key, Text = item.Value });
            }
            modelTypeList.Insert(0, new SelectListItem() { Value = "", Text = "==请选择==" });
            ViewBag.selectList = modelTypeList;

            ViewBag.OpType = type;
            var backupDevice = new BackupDevice();
            switch (type)
            {

                case OpType.Add:
                    break;
                case OpType.View:
                case OpType.Update:
                    backupDevice = backupdevicemanager.GetEntity(id);
                    break;
                default:
                    break;
            }

            return PartialView(backupDevice);

        }

        [HttpPost]
        public ActionResult AddOrEdit(OpType optype, BackupDevice backupdevice)
        {
            return base.ExecutionMethod(() =>
            {
                var model=backupdevicemanager.GetEntity(backupdevice.Id);
                string errorMsg = string.Empty;
                backupdevice.IsUsed = model.IsUsed;
                backupdevice.Model = model.Model;
                backupdevice.LastUpdateTime = DateTime.Now;
                backupdevice.LastUpdatUser = CurrentUser.ChineseName;
                backupdevicemanager.Update(backupdevice);
                return this.Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
            });
        }
        [HttpGet]
        [AjaxOnly]
        public ActionResult ChangeUseState(string deviceSerice, string deviceType, string hotelId, string hotelName)
        {
            ViewBag.OpType = OpType.Update;
            ViewBag.HotelName = hotelName;

            DeviceTrace device = new DeviceTrace();
            device.DeviceSeries = deviceSerice;
            device.DeviceType = deviceType;
            device.HotelId = hotelId;
            return PartialView(device);
        }
        [HttpPost]
        [AjaxOnly]
        public JsonResult ChangeUseState(DeviceTrace device)
        {
            //转为常用机
            BackupDevice backup = backupdevicemanager.Search(new BackupDeviceCriteria() { DeviceSeries = device.DeviceSeries}).First();
            if (backup.IsUsed)
            {
                return Json("该设备已转为常用机!");
            }
            if (string.IsNullOrEmpty(device.RoomNo))
            {
                return Json("房间号不能为空!");
            }
            return base.ExecutionMethod(() =>
            {
                device.DeviceType = backup.DeviceType;
                device.Active = true;
                device.Model = backup.Model;
                
                string message = AddDeviceTrace(device);
                if (message == "Success")
                {
                    backup.IsUsed = true;
                    backup.Active = true;
                    backup.LastUpdateTime=DateTime.Now;
                    backup.LastUpdatUser = base.CurrentUser.Account;
                    backupdevicemanager.Update(backup);
                }
                return Json(message);
            }, false);
        }

        public string GetHotelNameById(string hotelId)
        {
            if (hotelManager.GetHotelObject(hotelId).Hotel != null)
            {
                return hotelManager.GetHotelObject(hotelId).Hotel.HotelName;
            }
            return "";
        }
        private string AddDeviceTrace(DeviceTrace dto)
        {
            var exitAppversions = deviceTrancemanager.Search(new TraceCriteria { DeviceSeries = dto.DeviceSeries });

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
            deviceTrancemanager.Add(dto);
            return "Success";
        }
    }
}

