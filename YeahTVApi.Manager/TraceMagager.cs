namespace YeahTVApi.Manager
{
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Entity;
    using YeahTVApi.Infrastructure;
    using System.Collections.Generic;
    using System.Linq;
    using YeahTVApi.DomainModel.Mapping;
    using YeahTVApiLibrary.Infrastructure;
    using YeahTVLibrary.Manager;
    using YeahTVApiLibrary.Manager;

    public class TraceMagager : DeviceTraceManager, ITraceManager
    {
        private ITVTraceRepertory traceRepertory;
        private IRedisCacheService redisCacheManager;
        private IAppLibraryManager appLibraryManager;
        private IAppPublishLibraryRepertory appPublishRepertory;
        private IBackupDeviceRepertory backupDeviceRepertory;
        private IMongoDeviceTraceRepository mongoDeviceTraceRepository;
        private BackupDeviceManager backupDeviceManager;

        public TraceMagager(ITVTraceRepertory traceRepertory,
            IRedisCacheService redisCacheManager,
            IAppLibraryManager _appLibraryManager,
            IAppPublishLibraryRepertory appPublishRepertory,
            BackupDeviceManager _backupDeviceManager, IMongoDeviceTraceRepository mongoDeviceTraceRepository
            )
            : base(traceRepertory, _appLibraryManager, appPublishRepertory, _backupDeviceManager
            , redisCacheManager, mongoDeviceTraceRepository
            )
        {
            this.traceRepertory = traceRepertory;
            this.appLibraryManager = _appLibraryManager;
            this.appPublishRepertory = appPublishRepertory;
            this.redisCacheManager = redisCacheManager;
            this.backupDeviceManager = _backupDeviceManager;
        }

        public DeviceTrace GetDevice(RequestHeader header)
        {
            var trace = SearchTrace(header);
            DeviceTrace device =null;
            if (trace != null && trace.Any())
            {
                device = trace.FirstOrDefault();
            }
            return device;
        }

        #region pravite method

        private void GetTrace(RequestHeader header)
        {
            var trace = SearchTrace(header);

            if (trace != null && trace.Any())
            {
                header.HotelID = trace.FirstOrDefault().HotelId;
                header.RoomNo = trace.FirstOrDefault().RoomNo;
            }

            redisCacheManager.Set(header.DEVNO, header.HotelID + ";" + header.RoomNo);
        }

        private List<DeviceTrace> SearchTrace(RequestHeader header)
        {
            var criteria = new TraceCriteria();

            criteria.Platfrom = header.Platform;
            criteria.DeviceSeries = header.DEVNO;

            var trace = traceRepertory.Search(criteria);
            return trace;
        }

        #endregion
    }
}
