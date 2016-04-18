namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApiLibrary.Infrastructure;
    using System.Collections.Generic;
    using System.Linq;
    using YeahTVApi.DomainModel;
    using YeahTVApi.DomainModel.Models.DataModel;

    public class DeviceAppsMonitorRepertory : BaseRepertory<DeviceAppsMonitor, string>, IDeviceAppsMonitorRepertory
    {
        public override List<DeviceAppsMonitor> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as DeviceAppsMonitorCriteria;

            var query = base.Entities.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.Id))
                query = query.Where(q => q.Id.Equals(criteria.Id));

            if (!string.IsNullOrEmpty(criteria.VersionCode))
                query = query.Where(q => q.VersionCode.Equals(criteria.VersionCode));

            if (!string.IsNullOrEmpty(criteria.DeviceSeries))
                query = query.Where(q => q.DeviceSeries.Equals(criteria.DeviceSeries));

            if (!string.IsNullOrEmpty(criteria.PackageName))
                query = query.Where(q => q.PackageName.Equals(criteria.PackageName));

            if (!string.IsNullOrEmpty(criteria.Action))
                query = query.Where(q => q.Action.Equals(criteria.Action));

            if (criteria.Active.HasValue)
                query = query.Where(q => q.Active.Equals(criteria.Active.Value));

            return query.ToList();
        }

        public List<DeviceAppsMonitoApiMode> SearchDeviceAppsMonitorResponse(string deviceSeries, List<AppListRequestModel> appListRequestModels)
        {
            var packageNames = appListRequestModels.Select(a => a.PackageName);
            var versionCodes = appListRequestModels.Select(a => a.VersionCode);

            var query = (from deviceAppsMonitor in base.Entities
                         join app in base.Context.Set<Apps>()
                             on deviceAppsMonitor.PackageName equals app.PackageName
                         join appVersion in base.Context.Set<AppVersion>()
                             on app.Id equals appVersion.Id
                         where packageNames.Contains(app.PackageName)
                               && deviceAppsMonitor.DeviceSeries.Equals(deviceSeries) 
                               && !app.IsSystem
                         select new DeviceAppsMonitoApiMode
                         {
                             Action = deviceAppsMonitor.Action,
                             Active = deviceAppsMonitor.Active,
                             AppUrl = appVersion.AppUrl,
                             DeviceSeries = deviceAppsMonitor.DeviceSeries,
                             PackageName = deviceAppsMonitor.PackageName,
                             UpdateTime = deviceAppsMonitor.UpdateTime,
                             VersionCode = deviceAppsMonitor.VersionCode
                         }).Distinct();

            return query.ToList();
        }
    }
}
