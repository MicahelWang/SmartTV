namespace YeahTVApiLibrary.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.Models.DataModel;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApiLibrary.Infrastructure;
    using System.Linq;
    using YeahTVApi.Entity;
    using YeahTVApi.DomainModel.Enum;
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel;

    public class DeviceAppsMonitorManager : BaseManager<DeviceAppsMonitor,DeviceAppsMonitorCriteria>, IDeviceAppsMonitorManager
    {
        private IAppLibraryManager appLibraryManager;

        public DeviceAppsMonitorManager(IDeviceAppsMonitorRepertory deviceAppsMonitorRepertory, IAppLibraryManager appLibraryManager)
            : base(deviceAppsMonitorRepertory)
        {
            this.appLibraryManager = appLibraryManager;
        }

        public List<DeviceAppsMonitoApiMode> SearchDeviceAppsMonitorResponse(RequestHeader header, List<AppListRequestModel> appListRequestModels)
        {
            try
            {
                var deviceAppsMonitors = new List<DeviceAppsMonitor>();
                var deviceAppsMonitorCriteria = new DeviceAppsMonitorCriteria { DeviceSeries = header.DEVNO };
                var hotelExitApps = base.ModelRepertory.Search(deviceAppsMonitorCriteria);

                appListRequestModels.ForEach(a =>
                {
                    var appsCount = hotelExitApps.Where(h => h.PackageName.Equals(a.PackageName) && h.VersionCode.Equals(a.VersionCode)).Count();
                    if (appsCount == 0)
                    {
                        deviceAppsMonitors.Add(new DeviceAppsMonitor
                        {
                            PackageName = a.PackageName,
                            VersionCode = a.VersionCode,
                            VersionName = a.VersionName,
                            Active = true,
                            DeviceSeries = header.DEVNO,
                            UpdateTime = DateTime.Now,
                            Action = AppActionType.Install.ToString()
                        });
                    }
                });

                if (deviceAppsMonitors.Any())
                    base.ModelRepertory.Insert(deviceAppsMonitors);

                var deviceAppsMonitorResponses = (base.ModelRepertory as IDeviceAppsMonitorRepertory).SearchDeviceAppsMonitorResponse(header.DEVNO,appListRequestModels);

                var deviceAppsMonitorResponsesShouldReturn = new List<DeviceAppsMonitoApiMode>();

                if (deviceAppsMonitorResponses != null && deviceAppsMonitorResponses.Any())
                {
                    appListRequestModels.ForEach(a =>
                    {
                        var add = deviceAppsMonitorResponses
                            .Where(d => d.PackageName.Equals(a.PackageName) && d.VersionCode.Equals(a.VersionCode) && d.DeviceSeries.Equals(header.DEVNO)).Distinct();
                        deviceAppsMonitorResponsesShouldReturn.AddRange(add);
                    });               
                }

                deviceAppsMonitorResponsesShouldReturn = deviceAppsMonitorResponsesShouldReturn.Distinct().ToList();

                var publishApps = appLibraryManager.SearchAppPublishs(new AppPublishCriteria
                {
                    Active = true,
                    HotelId = header.HotelID,
                    PublishTime = DateTime.Now
                }).Where(a => !a.AppVersion.App.IsSystem).ToList();
                    

                publishApps.ForEach(p => 
                {
                    if (!deviceAppsMonitorResponsesShouldReturn.Any(d => d.PackageName.Equals(p.AppVersion.App.PackageName) && d.VersionCode.Equals(p.VersionCode)))
                        deviceAppsMonitorResponsesShouldReturn.Add(new DeviceAppsMonitoApiMode 
                        {
                            Action = AppActionType.Install.ToString(),
                            Active = true,
                            AppUrl = p.AppVersion.AppUrl,
                            DeviceSeries = header.DEVNO,
                            PackageName = p.AppVersion.App.PackageName,
                            UpdateTime = p.PublishDate,
                            VersionCode = p.VersionCode
                        });
                });

                deviceAppsMonitorResponsesShouldReturn = deviceAppsMonitorResponsesShouldReturn.GroupBy(p => p.PackageName).Select(s => s.OrderByDescending(o => o.VersionCode).FirstOrDefault()).ToList();
                return deviceAppsMonitorResponsesShouldReturn;

            }
            catch (Exception ex)
            {
                throw new CommonFrameworkManagerException("设置APP版本失败", ex);
            }
        }
    }
}