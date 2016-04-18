namespace YeahTVApiLibrary.Infrastructure
{
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Entity;
    using System.Collections.Generic;
    using System;
    using YeahTVApi.DomainModel.Models.ViewModels;
    using YeahTVApi.Common;

    public interface IDeviceTraceLibraryManager : IBaseManager<DeviceTrace, DeviceTraceCriteria>
    {
        [Cache]
        List<DeviceTrace> SearchOrderByRoomNo(DeviceTraceCriteria criteria);
       
        List<DeviceTraceForSP> LogDeviceTrace(RequestHeader header, out int status, out string tvKey);

        [Cache]
        string GetDevicePrivateKey(RequestHeader header);

        List<DeviceTraceForSP> LogDeviceTraceShouldNotCheckBind(RequestHeader header);

        [Cache]
        List<string> GetDeviceSeriesWithBackupDevice(string hotelId);

        [Cache]
        string GetAppKey(RequestHeaderBase header);

        [Cache]
        DeviceTrace GetSingle(DeviceTraceCriteria searchCriteria);

        [Cache]
        DeviceTrace GetAppTrace(RequestHeaderBase header);
        List<HotelInfoStatistics> GetDeviceTraceStatistics(List<string> hotelList);

        List<DeviceTraceForSP> GetConfigData(string packageName, RequestHeader header);
        void InitializeLogDeviceTrace(RequestHeader header);
    }
}
