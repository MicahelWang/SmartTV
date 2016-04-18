namespace YeahTVLibrary.Manager
{
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Entity;
    using YeahTVApiLibrary.Infrastructure;
    using System.Collections.Generic;
    using System.Linq;
    using YeahTVApi.DomainModel.Mapping;
    using System;
    using YeahTVApi.DomainModel;
    using YeahTVApiLibrary.Manager;
    using YeahTVApi.DomainModel.Models.ViewModels;

    public class DeviceTraceManager : BaseManager<DeviceTrace,DeviceTraceCriteria>, IDeviceTraceLibraryManager
    {
        private IDeviceTraceLibraryRepertory traceRepertory;
        private IAppLibraryManager appLibraryManager;
        private IAppPublishLibraryRepertory appPublishRepertory;
        private IBackupDeviceManager backupDeviceManager;
        private IMongoDeviceTraceRepository mongoDeviceTraceRepository;
        private readonly IRedisCacheService _redisCacheService;
       

        public DeviceTraceManager(IDeviceTraceLibraryRepertory traceRepertory,
            IAppLibraryManager appLibraryManager, IAppPublishLibraryRepertory appPublishRepertory
            , IBackupDeviceManager backupDeviceManager, IRedisCacheService redisCacheService
            , IMongoDeviceTraceRepository mongoDeviceTraceRepository)
            : base(traceRepertory)
        {
            this.traceRepertory = traceRepertory;
            this.appLibraryManager = appLibraryManager;
            this.appPublishRepertory = appPublishRepertory;
            this._redisCacheService = redisCacheService;
            this.backupDeviceManager = backupDeviceManager;
            this.mongoDeviceTraceRepository = mongoDeviceTraceRepository;
        }

        public virtual List<DeviceTrace> SearchOrderByRoomNo(DeviceTraceCriteria criteria)
        {
            return traceRepertory.SearchOrderByRoomNo(criteria);
        }

        public override void Update(DeviceTrace trace)
        {
            try
            {
                var traceInDb = traceRepertory.Search(new DeviceTraceCriteria { DeviceSeries = trace.DeviceSeries }).FirstOrDefault();
                //trace.CopyTo(traceInDb);
                traceInDb.RoomNo = trace.RoomNo;
                traceInDb.Remark = trace.Remark;
                traceInDb.HotelId = trace.HotelId;
                traceInDb.Active = trace.Active;
                traceInDb.DeviceType = trace.DeviceType;
                if (!string.IsNullOrEmpty(trace.ModelId.ToString()))
                {
                    traceInDb.ModelId = (int)trace.ModelId;
                }
                else
                {
                    traceInDb.ModelId = null;
                }
                traceRepertory.Update(traceInDb);
            }
            catch (Exception ex)
            {
                throw new CommonFrameworkManagerException("UpdateTrace error!", ex);
            }
        }

        public override void Delete(DeviceTrace trace)
        {
            traceRepertory.Delete(m => m.DeviceSeries == trace.DeviceSeries);
        }

        public virtual string GetDevicePrivateKey(RequestHeader header)
        {
            var trace = header.ToTVTrace();

            if (string.IsNullOrEmpty(trace.DeviceKey))
                trace.DeviceKey = SecurityManager.GetRNGString(30, 2);

            var traceInDb = traceRepertory.Search(new DeviceTraceCriteria { DeviceSeries = header.DEVNO }).FirstOrDefault(); ;
            if (traceInDb != null)
            {
                traceInDb.DeviceKey = trace.DeviceKey;
                traceInDb.Ip = trace.Ip;
                traceInDb.OsVersion = trace.OsVersion;
                traceInDb.Brand = trace.Brand;
                traceInDb.Manufacturer = trace.Manufacturer;
                traceInDb.LastVisitTime = DateTime.Now;

                traceRepertory.Update(traceInDb);
            }

            return trace.DeviceKey;
        }

        public virtual List<DeviceTraceForSP> LogDeviceTrace(RequestHeader header, out int status, out string tvKey)
        {
            try
            {
                status = 0;

                var trace = header.ToTVTrace();

                if (string.IsNullOrEmpty(trace.DeviceKey))
                    trace.DeviceKey = SecurityManager.GetRNGString(30, 2);

                tvKey = trace.DeviceKey;

                var app = appLibraryManager.GetAppByKey(header.APP_ID);

                if (app == null)
                {
                    status = -1;
                    return new List<DeviceTraceForSP>();
                }

                if (!app.Active)
                    status = -2;

                var criteria = new TraceCriteria();
                criteria.DeviceSeries = header.DEVNO;

                // var traceInDb = traceRepertory.GetSingle(criteria); 
                var traceInDb = traceRepertory.Search(criteria).FirstOrDefault();
                if (traceInDb != null)
                {
                    traceInDb.DeviceKey = trace.DeviceKey;
                    traceInDb.Ip = trace.Ip;
                    traceInDb.OsVersion = trace.OsVersion;
                    traceInDb.Brand = trace.Brand;
                    traceInDb.Manufacturer = trace.Manufacturer;
                    traceInDb.LastVisitTime = DateTime.Now;

                    traceRepertory.Update(traceInDb);
                    mongoDeviceTraceRepository.Add(new MongoDeviceTrace() { DeviceSeries = traceInDb.DeviceSeries, HotelId = traceInDb.HotelId, VisitTime = Convert.ToDateTime(traceInDb.LastVisitTime) });
                }
                else
                {
                    backupDeviceManager.UpdateBackupDeviceBySeries(header.DEVNO);
                }

                var appPublishCriteria = new AppPublishCriteria
                {
                    PublishTime = DateTime.Now,
                    HotelId = header.HotelID,
                    Active = true,
                    AppId = header.APP_ID
                };

                var latestVersionApp = appLibraryManager.SearchAppPublishs(appPublishCriteria).OrderByDescending(o => o.VersionCode).FirstOrDefault();

                var deviceTraceForSPs = new List<DeviceTraceForSP>();

                if (latestVersionApp != null)
                {
                    deviceTraceForSPs.Add(new DeviceTraceForSP
                    {
                        DictCode = "appUpdateUrl",
                        DictValue = latestVersionApp.AppVersion.AppUrl,
                    });

                    deviceTraceForSPs.Add(new DeviceTraceForSP
                    {
                        DictCode = "updateInfo",
                        DictValue = latestVersionApp.AppVersion.Description
                    });

                    deviceTraceForSPs.Add(new DeviceTraceForSP
                    {
                        DictCode = "CODE",
                        DictValue = latestVersionApp.AppVersion.VersionCode.ToString()
                    });
                }

                return deviceTraceForSPs;
            }
            catch (Exception ex)
            {
                throw new CommonFrameworkManagerException("LogDeviceTrace error!", ex);
            }
        }

        public virtual List<DeviceTraceForSP> LogDeviceTraceShouldNotCheckBind(RequestHeader header)
        {
            var appPublishCriteria = new AppPublishCriteria
            {
                PublishTime = DateTime.Now,
                AppId = header.APP_ID,
                HotelId = header.HotelID,
                Active = true
            };

            var latestVersionApp = appLibraryManager.SearchAppPublishs(appPublishCriteria).OrderByDescending(o => o.VersionCode).FirstOrDefault();

            var deviceTraceForSPs = new List<DeviceTraceForSP>();

            if (latestVersionApp != null)
            {
                deviceTraceForSPs.Add(new DeviceTraceForSP
                {
                    DictCode = "appUpdateUrl",
                    DictValue = latestVersionApp.AppVersion.AppUrl,
                });

                deviceTraceForSPs.Add(new DeviceTraceForSP
                {
                    DictCode = "updateInfo",
                    DictValue = latestVersionApp.AppVersion.Description
                });

                deviceTraceForSPs.Add(new DeviceTraceForSP
                {
                    DictCode = "CODE",
                    DictValue = latestVersionApp.AppVersion.VersionCode.ToString()
                });
            }

            return deviceTraceForSPs;
        }

        public Tuple<List<string>, List<string>> DeviceSeriesFilter(List<string> appPublishDeviceSeries, string hotelId)
        {
            return traceRepertory.DeviceSeriesFilter(appPublishDeviceSeries, hotelId);
        }

        public List<string> GetDeviceSeriesWithBackupDevice(string hotelId)
        {
            return traceRepertory.GetDeviceSeriesWithBackupDevice(hotelId);
        }

        public DeviceTrace GetSingle(DeviceTraceCriteria searchCriteria)
        {
            return traceRepertory.GetSingle(searchCriteria);
        }

        /// <summary>
        /// 获取设备跟踪数据
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public virtual DeviceTrace GetAppTrace(RequestHeaderBase header)
        {
            if (string.IsNullOrEmpty(header.DEVNO) || string.IsNullOrEmpty(header.APP_ID))
                throw new CommonFrameworkManagerException("GetAppTrace Error DEVNO or APP_ID is empty!", null);

            return traceRepertory.Search(new DeviceTraceCriteria { DeviceSeries = header.DEVNO }).FirstOrDefault();
        }

        public virtual string GetAppKey(RequestHeaderBase header)
        {
            var deviceTrace = traceRepertory.Search(new DeviceTraceCriteria { DeviceSeries = header.DEVNO }).FirstOrDefault();
            return deviceTrace != null ? deviceTrace.DeviceKey : null;
        }

        public List<HotelInfoStatistics> GetDeviceTraceStatistics(List<string> hotelList)
        {
            var list = traceRepertory.GetBackupDeviceStatistics(hotelList);//.GroupBy(m => m.HotelId).ToList();
            var q = from p in list
                    group p by p.HotelId into g
                    select new HotelInfoStatistics() { HotelId = g.Key, DeviceTraceSeriesCount = g.Count() };
            return q.ToList();
        }



        public List<DeviceTraceForSP> GetConfigData(string packageName, RequestHeader header)
        {

            var appVer = appPublishRepertory.Search(new AppPublishCriteria { HotelId = header.HotelID,Active=true,PublishTime=DateTime.Now }).
              Where(a => a.AppVersion.App.PackageName == packageName).
              OrderByDescending(a => a.VersionCode).
              FirstOrDefault();
             var deviceTraceForSPs = new List<DeviceTraceForSP>();

             if (appVer != null)
            {
                deviceTraceForSPs.Add(new DeviceTraceForSP
                {
                    DictCode = "launcherDownLoadUrl",
                    DictValue = appVer.AppVersion.AppUrl,
                });

                deviceTraceForSPs.Add(new DeviceTraceForSP
                {
                    DictCode = "launcherVersion",
                    DictValue = appVer.AppVersion.VersionCode.ToString()
                });

                deviceTraceForSPs.Add(new DeviceTraceForSP
                {
                    DictCode = "versionDescription",
                    DictValue = appVer.AppVersion.Description
                });
            }

            return deviceTraceForSPs;
             
        }


        public virtual void InitializeLogDeviceTrace(RequestHeader header)
        {
            try
            {
                var trace = header.ToTVTrace();

                if (string.IsNullOrEmpty(trace.DeviceKey))
                    trace.DeviceKey = SecurityManager.GetRNGString(30, 2);

                var criteria = new TraceCriteria {DeviceSeries = header.DEVNO};

                var traceInDb = traceRepertory.Search(criteria).FirstOrDefault();
                if (traceInDb != null)
                {
                    traceInDb.DeviceKey = trace.DeviceKey;
                    traceInDb.Ip = trace.Ip;
                    traceInDb.OsVersion = trace.OsVersion;
                    traceInDb.Brand = trace.Brand;
                    traceInDb.Manufacturer = trace.Manufacturer;
                    traceInDb.LastVisitTime = DateTime.Now;

                    traceRepertory.Update(traceInDb);
                    mongoDeviceTraceRepository.Add(new MongoDeviceTrace() { DeviceSeries = traceInDb.DeviceSeries, HotelId = traceInDb.HotelId, VisitTime = Convert.ToDateTime(traceInDb.LastVisitTime) });
                }
                else
                {
                    backupDeviceManager.UpdateBackupDeviceBySeries(header.DEVNO);
                }
            }
            catch (Exception ex)
            {
                throw new CommonFrameworkManagerException("LogDeviceTrace error!", ex);
            }
        }
    }
}
