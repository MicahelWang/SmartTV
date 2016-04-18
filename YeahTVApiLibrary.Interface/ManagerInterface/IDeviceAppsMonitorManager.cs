namespace YeahTVApiLibrary.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.Models.DataModel;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Entity;

    public interface IDeviceAppsMonitorManager : IBaseManager<DeviceAppsMonitor, DeviceAppsMonitorCriteria>
    {
        /// <summary>
        /// 此方法不能缓存
        /// </summary>
        /// <param name="header"></param>
        /// <param name="appListRequestModels"></param>
        /// <returns></returns>
        List<DeviceAppsMonitoApiMode> SearchDeviceAppsMonitorResponse(RequestHeader header, List<AppListRequestModel> appListRequestModels);
    }
}
