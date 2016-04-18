using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.Entity;
using YeahTVApiLibrary.Controllers;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Filter;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.Common;
using System.IO;
using YeahTVApi.DomainModel;
using YeahAppCentre.Web.Utility;
using YeahTVApi.Filter;

namespace YeahTVApi.Controllers
{
    public class BackupDeviceController : BaseController
    {
        // GET: BackupDevice
        private IBackupDeviceManager backupdevicemanager;
        private ILogManager logManager;
        private IConstantSystemConfigManager constantSystemConfigManager;
        private IRequestApiService requestApiService;
        private IDeviceTraceLibraryManager traceManager;
        public BackupDeviceController(IBackupDeviceManager backupdevicemanager,
            ILogManager logManager,
            IConstantSystemConfigManager constantSystemConfigManager,
            IRequestApiService requestApiService,
            IDeviceTraceLibraryManager traceManager)
        {
            this.requestApiService = requestApiService;
            this.logManager = logManager;
            this.backupdevicemanager = backupdevicemanager;
            this.constantSystemConfigManager = constantSystemConfigManager;
            this.traceManager = traceManager;
        }
        [HTWebFilterAttribute]
        public ApiListResult<BackupDevice> GetListBackUpDeviceTrace(string HotelId, int PageIndex, int PageSize = 10)
        {
            var listDeviceTrace = backupdevicemanager.Search(new BackupDeviceCriteria { HotelId = HotelId, Page = PageIndex, PageSize = PageSize });
            return new ApiListResult<BackupDevice> { list = listDeviceTrace };
        }

        [HTWebFilterAttribute]
        public ApiResult AddDeviceObj(BackupDevice trace)
        {
            string error = string.Empty;
            var res = new ApiResult();
            try
            {
                if (AddBackUpDeviceTrace(trace, ref error))
                {
                    return res.WithOk();
                }
                return res.WithError(error);
            }
            catch (Exception ex)
            {
                logManager.SaveError("添加失败:" + ex.Message.ToString(), ex, AppType.CommonFramework);
                return res.WithError(ex.ToString());
            }
        }

        private bool AddBackUpDeviceTrace(BackupDevice dto, ref string error)
        {
            var exitBackupdevice = backupdevicemanager.SearchFromCache(new BackupDeviceCriteria { DeviceSeries = dto.DeviceSeries }).ToList();

            if (exitBackupdevice != null && exitBackupdevice.Any())
            {
                error = string.Format("该设备已绑定为 {0} 的备用设备，无法重复进行绑定！", GetHotelNameByHotelId(exitBackupdevice[0].HotelId));
                return false;
            }

            var exitdevices = traceManager.SearchFromCache(new DeviceTraceCriteria { DeviceSeries = dto.DeviceSeries }).ToList();

            if (exitdevices != null && exitdevices.Any())
            {
                error = String.Format("该设备已经被绑定到酒店{1}{0}房间，无法再次进行绑定！", exitdevices[0].RoomNo, GetHotelNameByHotelId(exitdevices[0].HotelId));
                return false;
            }

            dto.LastUpdateTime = DateTime.Now;
            dto.LastUpdatUser = Header.Guest;
            backupdevicemanager.Add(dto);
            return true;
        }

        public string GetHotelNameByHotelId(string hotelId)
        {
            var hoteLUrl = constantSystemConfigManager.AppCenterUrl + Constant.GetHotelApiUrl + hotelId;
            var hotelEf = requestApiService.HttpRequest(hoteLUrl, "GET").JsonStringToObj<Hotel>();
            return hotelEf == null ? "" : hotelEf.hotelName;
        }
    }
}