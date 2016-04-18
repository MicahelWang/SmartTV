using System;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApiLibrary.Infrastructure;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using YeahTVApi.Common;
    using EntityFramework.Extensions;

    public class DeviceTraceRepertory : BaseRepertory<DeviceTrace, string>, IDeviceTraceLibraryRepertory
    {

        public override List<DeviceTrace> Search(BaseSearchCriteria searchCriteria)
        {
            var deviceSearchCriteria = searchCriteria as DeviceTraceCriteria;
            var query = base.Entities.AsQueryable();


            if (!string.IsNullOrEmpty(deviceSearchCriteria.DeviceSeries))
                query = query.Where(q => q.DeviceSeries.Contains(deviceSearchCriteria.DeviceSeries));

            if (!string.IsNullOrEmpty(deviceSearchCriteria.Platfrom))
                query = query.Where(q => q.Platfrom.Contains(deviceSearchCriteria.Platfrom));

            if (!string.IsNullOrEmpty(deviceSearchCriteria.HotelId))
                query = query.Where(q => q.HotelId.Equals(deviceSearchCriteria.HotelId));

            if (!string.IsNullOrEmpty(deviceSearchCriteria.RoomNo))
            {
                query = query.Where(q => q.RoomNo.Contains(deviceSearchCriteria.RoomNo));
            }

            if (deviceSearchCriteria.BeginTime.HasValue)
            {
                var startTime = deviceSearchCriteria.BeginTime.Value;
                query = query.Where(q => q.LastVisitTime >= startTime);
            }

            if (deviceSearchCriteria.EndTime.HasValue)
            {
                var endTime = deviceSearchCriteria.EndTime.Value.AddDays(1);
                query = query.Where(q => q.LastVisitTime <= endTime);
            }

            if (deviceSearchCriteria.DeviceType.HasValue)
            {
                query = query.Where(q => q.DeviceType.Equals(deviceSearchCriteria.DeviceType.Value.ToString()));
            }

            if (deviceSearchCriteria.Active.HasValue)
            {
                query = query.Where(q => q.Active.Equals(deviceSearchCriteria.Active.Value));
            }

            if (searchCriteria.SortFiled.Equals("Id"))
                searchCriteria.SortFiled = "RoomNo";

            return query.ToPageList(deviceSearchCriteria);
        }

        /// <summary>
        /// 针对roomNo排序暂用方法，需优化
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public List<DeviceTrace> SearchOrderByRoomNo(BaseSearchCriteria searchCriteria)
        {
            var deviceSearchCriteria = searchCriteria as DeviceTraceCriteria;
            var query = base.Entities.AsQueryable();
            searchCriteria.SortFiled = "RoomNo";

            var queryStr = query.Where(m => m.HotelId.Equals(deviceSearchCriteria.HotelId)).ToPageListQueryable(searchCriteria);
            var resultQuery = base.Entities.SqlQuery(queryStr.ToString().
                Replace("`RoomNo` DESC", "`RoomNo`*1 DESC").Replace("`RoomNo` ASC", "`RoomNo`*1 ASC").Replace("@p__linq__0", "'" + deviceSearchCriteria.HotelId + "'"));

            return resultQuery.ToList();
        }

        public DeviceTrace GetSingle(BaseSearchCriteria searchCriteria)
        {

            var deviceSearchCriteria = searchCriteria as DeviceTraceCriteria;
            var query = base.Entities.AsQueryable();

            if (!string.IsNullOrEmpty(deviceSearchCriteria.DeviceSeries))
                query = query.Where(q => q.DeviceSeries.Equals(deviceSearchCriteria.DeviceSeries));

            if (!string.IsNullOrEmpty(deviceSearchCriteria.Platfrom))
                query = query.Where(q => q.Platfrom.Equals(deviceSearchCriteria.Platfrom));

            if (!string.IsNullOrEmpty(deviceSearchCriteria.HotelId))
                query = query.Where(q => q.HotelId.Contains(deviceSearchCriteria.HotelId));

            return query.SingleOrDefault();
        }

        public Tuple<List<string>, List<string>> DeviceSeriesFilter(List<string> appPublishDeviceSeries, string hotelId)
        {
            var query = Entities.Where(m => m.HotelId == hotelId).Select(m => m.DeviceSeries)
                .Union(Context.Set<BackupDevice>().Where(m => m.HotelId == hotelId).Select(m => m.DeviceSeries));

            var updateList = query.Where(m => appPublishDeviceSeries.Contains(m)).ToList();
            var insertList = appPublishDeviceSeries.Except(updateList).ToList();

            return new Tuple<List<string>, List<string>>(insertList, updateList);
        }
        public List<string> GetDeviceSeriesWithBackupDevice(string hotelId)
        {
            var query = Entities.Where(m => m.HotelId == hotelId).Select(m => m.DeviceSeries)
                .Union(Context.Set<BackupDevice>().Where(m => m.HotelId == hotelId).Select(m => m.DeviceSeries));

            return query.ToList();
        }


        public List<DeviceTrace> GetBackupDeviceStatistics(List<string> hotelList)
        {
            return base.Entities.Where(m => hotelList.Contains(m.HotelId)).ToList();
        }
    }
}
