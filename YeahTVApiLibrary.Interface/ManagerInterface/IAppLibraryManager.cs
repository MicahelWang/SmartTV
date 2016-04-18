namespace YeahTVApiLibrary.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Entity;

    public interface IAppLibraryManager : IBaseManager<Apps, AppsCriteria>
    {
        [Cache]
        BackupDevice GetAppBackupDevice(RequestHeaderBase header);

        Dictionary<String, Apps> GetAppVersionList();

        Hotel QueryHotel(string HotelID);

        Apps GetAppByAppId(string appId);

        Apps GetAppByKey(string appId);

        List<Apps> SearchAppsFromCache(string appName, string appVersion, string packageName = null);

        List<AppVersion> SearchAppVersions(AppsCriteria appsCriteria);

        List<AppPublish> SearchAppPublishs(AppPublishCriteria appPublishCriteria);

        void AddVersion(AppVersion appVersion);

        void AddPublish(AppPublish appPublish);

        void UpdateAppVersion(AppVersion appVersion);

        void UpdateAppPublish(AppPublish appPublish);
    }
}
