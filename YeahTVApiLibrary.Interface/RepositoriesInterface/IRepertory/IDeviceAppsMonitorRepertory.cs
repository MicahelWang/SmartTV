namespace YeahTVApiLibrary.Infrastructure
{
    using System.Collections.Generic;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.Models.DataModel;
    using YeahTVApi.DomainModel.SearchCriteria;

    public interface IDeviceAppsMonitorRepertory : IBsaeRepertory<DeviceAppsMonitor>
    {
        List<DeviceAppsMonitoApiMode> SearchDeviceAppsMonitorResponse(string deviceSeries, List<AppListRequestModel> appListRequestModels);
    }
}
