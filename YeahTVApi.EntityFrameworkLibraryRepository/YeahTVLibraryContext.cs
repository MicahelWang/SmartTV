using YeahTVApi.DomainModel.Models.DomainModels;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Models
{
    using YeahTVApi.DomainModel.Models;
    using YeahTVApiLibrary.EntityFrameworkRepository.Mapping;
    using System.Data.Entity;

    public partial class YeahTVLibraryContext : DbContext
    {
        static YeahTVLibraryContext()
        {
            Database.SetInitializer<YeahTVLibraryContext>(null);
        }

        public YeahTVLibraryContext()
            : base("Name=YeahTVContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        public YeahTVLibraryContext(string connectionStrings)
            : base(connectionStrings)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<AppPublish> AppPublishes { get; set; }
        public DbSet<Apps> Apps { get; set; }
        public DbSet<AppVersion> AppVersions { get; set; }
        public DbSet<BackupDevice> BackupDevices { get; set; }
        public DbSet<BehaviorLog> BehaviorLogs { get; set; }
        public DbSet<DeviceAppsMonitor> DeviceAppsMonitors { get; set; }
        public DbSet<DeviceTrace> DeviceTraces { get; set; }
        public DbSet<HotelTVChannel> HotelTvChannels { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<SystemConfig> SystemConfigs { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }
        public DbSet<TVChannel> TvChannels { get; set; }
        public DbSet<VODRecord> VODRecords { get; set; }
        public DbSet<MovieTemplate> MovieTemplates { get; set; }
        public DbSet<MovieTemplateRelation> MovieTemplateRelations { get; set; }
        public DbSet<HotelMovieTrace> HotelMovieTraces { get; set; }
        public DbSet<CoreSysAttachment> CoreSysAttachments { get; set; }
        public DbSet<TVHotelConfig> TVHotelConfigs { get; set; }
        public DbSet<AuthCertigierManager> AuthCertigierManagers { get; set; }
        public DbSet<AuthUserDeviceTrace> AuthUserDeviceTraces { get; set; }

        public DbSet<CoreSysLogin> CoreSysLogins { get; set; }
        public DbSet<ErpSysUser> ErpSysUsers { get; set; }
        public DbSet<ErpPowerResource> ErpPowerResources { get; set; }
        public DbSet<ErpPowerRole> ErpPowerRoles { get; set; }
        public DbSet<ErpPowerRoleResourceRelation> ErpPowerRoleResourceRelations { get; set; }
        public DbSet<ErpSysHotelManage> ErpSysHotelManages { get; set; }
        public DbSet<ErpSysRelationManage> ErpSysRelationManages { get; set; }
        public DbSet<CoreSysOtherLogin> CoreSysOtherLogins { get; set; }
        public DbSet<VODOrder> VODOrders { get; set; }
        public DbSet<VODPaymentRequest> VODPaymentRequests { get; set; }
        public DbSet<VODPaymentResult> VODPaymentResults { get; set; }
        public DbSet<VODRequest> VODRequests { get; set; }
        public DbSet<CatchHotel> CatchHotels { get; set; }
        public DbSet<MovieForLocalize> MovieForLocalizes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<HotelMovieTraceNoTemplate> HotelMovieTraceNoTemplates { get; set; }
        public DbSet<LocalizeResource> LocalizeResources { get; set; }

        public DbSet<HCSDownloadTask> HCSDownloadTask { get; set; }

        public DbSet<HCSDownLoadJob> HCSDownLoadJob { get; set; }

        public DbSet<HCSConfig> HCSConfig { get; set; }

        public DbSet<HotelPermition> HotelPermitions { get; set; }
        public DbSet<OrderProducts> OrderProducts { get; set; }
        public DbSet<StoreOrder> StoreOrder { get; set; }
        public DbSet<HCSCacheVersion> HCSCacheVersion { get; set; }
        public DbSet<GlobalConfig> GlobalConfig { get; set; }


        public DbSet<AuthTVToken> AuthTVToken { get; set; }
        public DbSet<ScoreExchang> ScoreExchang { get; set; }
        public DbSet<OrderQRCodeRecord> OrderQRCodeRecord { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new OrderProductsMap());
            modelBuilder.Configurations.Add(new StoreOrderMap());

            modelBuilder.Configurations.Add(new HotelPermitionMap());
            modelBuilder.Configurations.Add(new CatchHotelMap());
            modelBuilder.Configurations.Add(new AppPublishMap());
            modelBuilder.Configurations.Add(new AppsMap());
            modelBuilder.Configurations.Add(new AppVersionMap());
            modelBuilder.Configurations.Add(new BackupDeviceMap());
            modelBuilder.Configurations.Add(new BehaviorLogMap());
            modelBuilder.Configurations.Add(new DeviceAppsMonitorMap());
            modelBuilder.Configurations.Add(new DeviceTraceMap());
            modelBuilder.Configurations.Add(new HotelTVChannelMap());
            modelBuilder.Configurations.Add(new MovieMap());
            modelBuilder.Configurations.Add(new SystemConfigMap());
            modelBuilder.Configurations.Add(new SystemLogMap());
            modelBuilder.Configurations.Add(new TVChannelMap());
            modelBuilder.Configurations.Add(new MovieTemplateMap());
            modelBuilder.Configurations.Add(new MovieTemplateRelationMap());
            modelBuilder.Configurations.Add(new MovieForLocalizeMap());
            modelBuilder.Configurations.Add(new TagMap());
            modelBuilder.Configurations.Add(new HotelMovieTraceNoTemplateMap());
            modelBuilder.Configurations.Add(new LocalizeResourceMap());
            modelBuilder.Configurations.Add(new HotelMovieTraceMap());
            modelBuilder.Configurations.Add(new VODRecordMap());
            modelBuilder.Configurations.Add(new CoreSysAttachmentMap());
            modelBuilder.Configurations.Add(new TvHotelConfigMap());
            modelBuilder.Configurations.Add(new AuthCertigierManagerMap());
            modelBuilder.Configurations.Add(new AuthUserDeviceTraceMap());
            modelBuilder.Configurations.Add(new GlobalConfigMap());

            modelBuilder.Configurations.Add(new ErpSysUserMap());
            modelBuilder.Configurations.Add(new ErpPowerResourceMap());
            modelBuilder.Configurations.Add(new ErpPowerRoleMap());
            modelBuilder.Configurations.Add(new ErpPowerRoleResourceRelationMap());
            modelBuilder.Configurations.Add(new ErpSysHotelManageMap());
            modelBuilder.Configurations.Add(new ErpSysRelationManageMap());
            modelBuilder.Configurations.Add(new CoreSysLoginMap());
            modelBuilder.Configurations.Add(new CoreSysOtherLoginMap());

            modelBuilder.Configurations.Add(new VODOrderMap());
            modelBuilder.Configurations.Add(new VODPaymentRequestMap());
            modelBuilder.Configurations.Add(new VODPaymentResultMap());
            modelBuilder.Configurations.Add(new VODRequestMap());

            modelBuilder.Configurations.Add(new HCSDownloadTaskMap());
            modelBuilder.Configurations.Add(new HCSDownLoadJobMap());
            modelBuilder.Configurations.Add(new HCSConfigMap());
            modelBuilder.Configurations.Add(new HCSCacheVersionMap());
            modelBuilder.Configurations.Add(new AuthTVTokenMap());
            modelBuilder.Configurations.Add(new ScoreExchangMap());
            modelBuilder.Configurations.Add(new OrderQRCodeRecordMap());

        }
    }
}
