using System;

namespace YeahTVApiLibrary.Manager
{
    using System.Collections.Generic;
    using System.Linq;
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Entity;
    using YeahTVApiLibrary.Infrastructure;

    /// <summary>
    /// 
    /// </summary>
    public class AppLibraryManager : BaseManager<Apps,AppsCriteria>, IAppLibraryManager
    {
        private IBackupDeviceManager backupDeviceManager;
        private IAppVersionLibraryRepertory appVersionRepertory;
        private IAppPublishLibraryRepertory appPublishLibraryRepertory;
        private IRedisCacheManager redisCacheManager;
        private ICacheManager cacheManager;
        private readonly IRedisCacheService _redisCacheService;

        public AppLibraryManager(
            IAppsLibraryRepertory appsRepertory,
            IRedisCacheManager redisCacheManager,
            IAppPublishLibraryRepertory appPublishLibraryRepertory,
            IAppVersionLibraryRepertory appVersionRepertory,
            ICacheManager cacheManager,
            IBackupDeviceManager backupDeviceManager,
             IRedisCacheService redisCacheService):base(appsRepertory)
        {
            this.backupDeviceManager = backupDeviceManager;
            this.redisCacheManager = redisCacheManager;
            this.appVersionRepertory = appVersionRepertory;
            this.appPublishLibraryRepertory = appPublishLibraryRepertory;
            this.cacheManager = cacheManager;
            _redisCacheService = redisCacheService;
        }

        public Dictionary<string, Apps> GetAppsFromCache()
        {
            if (!redisCacheManager.IsSet(Constant.AppsListKey))
            {
                cacheManager.SetAppsList();
            }
            return redisCacheManager.Get<Dictionary<string, Apps>>(Constant.AppsListKey);
        }

        public Apps GetAppByAppId(string appId)
        {
            return base.ModelRepertory.Search(new AppsCriteria { Id = appId }).FirstOrDefault();
        }
        public Apps GetAppByKey(string appKey)
        {
            Apps appEntity = null;
            GetAppsFromCache().TryGetValue(appKey, out appEntity);
            return appEntity;
        }

        public List<Apps> SearchAppsFromCache(string appName, string appVersion, string packageName = null)
        {
            var apps = GetAppsFromCache();
            if (apps != null && apps.Any())
            {
                var query = apps.Select(s => s.Value).AsEnumerable();

                if (!string.IsNullOrEmpty(appName))
                    query = query.Where(q => q.Name.Equals(appName));

                if (!string.IsNullOrEmpty(appVersion))
                    query = query.Where(q => q.AppVresions.Any(a => a.VersionCode.Equals(appVersion)));

                if (!string.IsNullOrEmpty(packageName))
                    query = query.Where(q => q.PackageName.Equals(packageName));

                return query.ToList();
            }
            else
            {
                return new List<Apps>();
            }
        }

   
        public BackupDevice GetAppBackupDevice(RequestHeaderBase header)
        {
            if (string.IsNullOrEmpty(header.DEVNO) || string.IsNullOrEmpty(header.APP_ID))
                throw new CommonFrameworkManagerException("GetAppBackupDevice Error DEVNO or APP_ID is empty!", null);

            return backupDeviceManager.SearchFromCache(new BackupDeviceCriteria { DeviceSeries = header.DEVNO }).FirstOrDefault();
        }

        /// <summary>
        /// 获取TV所有版本列表
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, Apps> GetAppVersionList()
        {
            return GetAppsFromCache();
        }

        public virtual Hotel QueryHotel(string HotelID)
        {
            var hotel = new Hotel();
            var json = redisCacheManager.GetCache(Constant.HtotelCacheKey + HotelID);

            hotel = string.IsNullOrEmpty(json) ? null : json.JsonStringToObj<Hotel>();

            return hotel;
        }

        public override void Update(Apps app)
        {
            var appDb = base.ModelRepertory.Search(new AppsCriteria { Id = app.Id}).FirstOrDefault();
            appDb = app.CopyTo<Apps>(appDb, new string[] { "Id", "AppKey", "SecureKey" });

            base.ModelRepertory.Update(app);
        }

        public List<AppVersion> SearchAppVersions(AppsCriteria appsCriteria)
        {
            return appVersionRepertory.Search(appsCriteria);
        }

        public void AddVersion(AppVersion appVersion)
        {
            appVersionRepertory.Insert(appVersion);
        }

        public void UpdateAppVersion(AppVersion appVersion)
        {
            var versionDb = appVersionRepertory
                .Search(new AppsCriteria
                {
                    Id = appVersion.Id,
                    AppVersion = appVersion.VersionCode,
                }).FirstOrDefault();

            versionDb = appVersion.CopyTo<AppVersion>(versionDb, new string[] { "Id" });

            appVersionRepertory.Update(versionDb);
        }


        public List<AppPublish> SearchAppPublishs(AppPublishCriteria appPublishCriteria)
        {
            return appPublishLibraryRepertory.Search(appPublishCriteria);
        }

        public void AddPublish(AppPublish appPublish)
        {
            appPublishLibraryRepertory.Insert(appPublish);
        }


        public void UpdateAppPublish(AppPublish appPublish)
        {
            var appPublishDb = appPublishLibraryRepertory
                .Search(new AppPublishCriteria
                {
                    AppId = appPublish.Id,
                    VersionCode = appPublish.VersionCode,
                    HotelId = appPublish.HotelId
                }).FirstOrDefault();

            if (appPublishDb != null)
            {
                appPublishDb.PublishDate = appPublish.PublishDate;
                appPublish.CopyTo(appPublishDb, new string[] { "Id" });
                appPublishLibraryRepertory.Update(appPublishDb);
            }
        }
    }
}
