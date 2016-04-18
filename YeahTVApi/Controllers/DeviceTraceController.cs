using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YeahAppCentre.Web.Utility;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.Entity;
using YeahTVApi.Filter;
using YeahTVApiLibrary.Controllers;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTVApi.Controllers
{
    public class DeviceTraceController : BaseController
    {
        private IDeviceTraceLibraryManager traceManager;
        private IAppLibraryManager appManager;
        private IFileUploadServiceManager fileUploadServiceManager;
        private ILogManager logManager;
        private IRedisCacheManager redisCacheManager;
        private IRequestApiService requestApiService;
        private IConstantSystemConfigManager constantSystemConfigManager;
        private IBackupDeviceManager backupDeviceManager;
        private ISysAttachmentManager sysAttachmentManager;

        public DeviceTraceController(IDeviceTraceLibraryManager traceManager,
            IAppLibraryManager appManager,
            IBackupDeviceManager backupDeviceManager,
            ILogManager logManager,
            IRequestApiService requestApiService,
            IHttpContextService httpContextService,
            IFileUploadServiceManager fileUploadServiceManager,
            IRedisCacheManager redisCacheManager,
            IConstantSystemConfigManager constantSystemConfigManager,
             ISysAttachmentManager sysAttachmentManager)
        {
            this.backupDeviceManager = backupDeviceManager;
            this.requestApiService = requestApiService;
            this.redisCacheManager = redisCacheManager;
            this.traceManager = traceManager;
            this.appManager = appManager;
            this.logManager = logManager;
            this.fileUploadServiceManager = fileUploadServiceManager;
            this.constantSystemConfigManager = constantSystemConfigManager;
            this.sysAttachmentManager = sysAttachmentManager;
        }
        // GET: DeviceTrace
        /// <summary>
        ///   根据酒店ID返回设备列表
        /// </summary>
        /// <returns></returns>
        [HTWebFilterAttribute]
        public ApiListResult<SimpDeviceTrace> GetListDeviceTrace(string HotelId, int PageIndex, int PageSize = 10)
        {
            var lisSimpDeviceTrace = new List<SimpDeviceTrace>();
            var listDeviceTrace = traceManager.SearchOrderByRoomNo(new DeviceTraceCriteria() { HotelId = HotelId, Page = PageIndex, PageSize = PageSize, SortFiled = "RoomNo", NeedPaging = true, OrderAsc = true });
            //listDeviceTrace.OrderBy(m =>Convert.ToInt32(m.RoomNo));
            foreach (var item in listDeviceTrace)
            {
                lisSimpDeviceTrace.Add(new SimpDeviceTrace()
                {
                    Active = item.Active,
                    Brand = item.Brand,
                    DeviceSeries = item.DeviceSeries,
                    DeviceType = item.DeviceType,
                    HotelId = item.HotelId,
                    Id = item.Id,
                    Ip = item.Ip,
                    LastVisitTime = item.LastVisitTime,
                    Manufacturer = item.Manufacturer,
                    Model = item.Model,
                    OsVersion = item.OsVersion,
                    Remark = item.Remark,
                    RoomNo = item.RoomNo,
                    Attachments = item.Attachments
                });
            }
            return new ApiListResult<SimpDeviceTrace> { list = lisSimpDeviceTrace };
        }

        public List<string> GetImageUrl(string attachments)
        {
            List<string> list = new List<string>();

            if (string.IsNullOrEmpty(attachments))
            {
                return list;
            }
            string[] strs = attachments.Split(',');
            int[] ids = new int[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                ids[i] = int.Parse(strs[i]);
            }
            var listAttachment = sysAttachmentManager.GetByIds(ids);
            foreach (var item in listAttachment)
            {
                list.Add(constantSystemConfigManager.ResourceSiteAddress + item.FilePath);
            }
            return list;
        }

        /* public int FuzhiByRef(List<DeviceTrace> listDevieTace)
         {
             Type deviceTraceType = typeof(DeviceTrace);
             Type simpDeviceTraceType = typeof(SimpDeviceTrace);
             //获取 实体类 所有的 公有属性
             List<PropertyInfo> proInfos = deviceTraceType.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
             List<PropertyInfo> simpDeviceTraceProInfos = simpDeviceTraceType.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();


             foreach (DeviceTrace item in listDevieTace)
             {
                 
             }


             //4.1查询要修改的数据
             List<P05MODEL.User> listModifing = db.Users.Where(whereLambda).ToList();

             //获取 实体类 类型对象
             Type t = typeof(P05MODEL.User); // model.GetType();
             //获取 实体类 所有的 公有属性
             List<PropertyInfo> proInfos = t.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
             //创建 实体属性 字典集合
             Dictionary<string, PropertyInfo> dictPros = new Dictionary<string, PropertyInfo>();
             //将 实体属性 中要修改的属性名 添加到 字典集合中 键：属性名  值：属性对象
             proInfos.ForEach(p =>
             {
                 if (modifiedProNames.Contains(p.Name))
                 {
                     dictPros.Add(p.Name, p);
                 }
             });

             //4.3循环 要修改的属性名
             foreach (string proName in modifiedProNames)
             {
                 //判断 要修改的属性名是否在 实体类的属性集合中存在
                 if (dictPros.ContainsKey(proName))
                 {
                     //如果存在，则取出要修改的 属性对象
                     PropertyInfo proInfo = dictPros[proName];
                     //取出 要修改的值
                     object newValue = proInfo.GetValue(model, null); //object newValue = model.uName;

                     //4.4批量设置 要修改 对象的 属性
                     foreach (P05MODEL.User usrO in listModifing)
                     {
                         //为 要修改的对象 的 要修改的属性 设置新的值
                         proInfo.SetValue(usrO, newValue, null); //usrO.uName = newValue;
                     }
                 }
             }
             //4.4一次性 生成sql语句到数据库执行
             return db.SaveChanges();
         }
         */



        [HTWebFilterAttribute]
        public ApiObjectResult<Dictionary<string, string>> GetListDeviceTraceModel()
        {
            var listDeviceTrace = new Dictionary<string, string> 
            {
                {DeviceType.AIO.ToString(), DeviceType.AIO.GetDescription().ToString()},
                {DeviceType.STB. ToString(),DeviceType.STB.GetDescription().ToString()}
            };
            return new ApiObjectResult<Dictionary<string, string>> { obj = listDeviceTrace };
        }

        [HTWebFilterAttribute]
        public ApiResult BindDevice(DeviceTrace trace)
        {
            trace.Active = true;
            string error = string.Empty;
            var res = new ApiResult();
            try
            {
                if (AddDeviceTrace(trace, ref error))
                {
                    return res.WithOk();
                }
                return res.WithError(error);
            }
            catch (Exception ex)
            {
                logManager.SaveError("添加失败", ex, AppType.CommonFramework);
                return res.WithError(ex.ToString());
            }
        }

        [HTWebFilterAttribute]
        public ApiObjectResult<string> UpLoadImage(Stream fileStream, string fileType)
        {
            ApiResult res = new ApiResult();
            var filePath = Constant.ResourceSiteAddress + fileUploadServiceManager.UpdateImageByBitmapStream(fileStream, fileType);
            return new ApiObjectResult<string> { obj = filePath };
        }

        private bool AddDeviceTrace(DeviceTrace dto, ref string error)
        {
            var exitdevices = traceManager.SearchFromCache(new DeviceTraceCriteria { DeviceSeries = dto.DeviceSeries }).ToList();

            if (exitdevices != null && exitdevices.Any())
            {
                error = String.Format("该设备已经被绑定到酒店{1}{0}房间，无法再次进行绑定！", exitdevices[0].RoomNo, GetHotelNameByHotelId(exitdevices[0].HotelId));
                return false;
            }

            var exibackupdevice = backupDeviceManager.SearchFromCache(new BackupDeviceCriteria { DeviceSeries = Header.DEVNO }).ToList();

            if (exibackupdevice != null && exibackupdevice.Any())
            {
                error = string.Format("该设备已绑定为 {0} 的备用设备，无法重复进行绑定！", GetHotelNameByHotelId(exibackupdevice[0].HotelId));
                return false;
            }

            var url = constantSystemConfigManager.AppCenterUrl + Constant.GetHotelApiUrl + dto.HotelId;
            var hotels = requestApiService.HttpRequest(url, "GET").JsonStringToObj<HotelEntity>();
            dto.GroupId = hotels.GroupId;
            dto.LastVisitTime = DateTime.Now;
            dto.FirstVisitTime = DateTime.Now;
            traceManager.Add(dto);
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