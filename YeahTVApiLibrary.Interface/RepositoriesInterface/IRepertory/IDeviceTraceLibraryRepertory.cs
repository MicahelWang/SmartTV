using System;

namespace YeahTVApiLibrary.Infrastructure
{
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using System.Collections.Generic;

    public interface IDeviceTraceLibraryRepertory : IBsaeRepertory<DeviceTrace>
    {
        DeviceTrace GetSingle(BaseSearchCriteria searchCriteria);
        Tuple<List<string>, List<string>> DeviceSeriesFilter(List<string> appPublishDeviceSeries, string hotelId);
        List<string> GetDeviceSeriesWithBackupDevice(string hotelId);

        List<DeviceTrace> SearchOrderByRoomNo(BaseSearchCriteria searchCriteria);

        List<DeviceTrace> GetBackupDeviceStatistics(List<string> hotelList);
    }
}
