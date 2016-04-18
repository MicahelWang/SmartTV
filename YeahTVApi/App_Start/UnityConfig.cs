using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Linq;
using YeahCentre.EntityFrameworkRepository.Repertory;
using YeahTVApi.Behavior;
using YeahTVApi.EntityFrameworkRepository.Repertory;
using YeahTVApi.Infrastructure;
using YeahTVApi.Manager;
using YeahTVApi.ServiceProvider;
using YeahTVApiLibrary;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Infrastructure.FactoryInterface;
using YeahTVApiLibrary.Infrastructure.ManagerInterface;
using YeahTVApiLibrary.Infrastructure.RepositoriesInterface.IRepertory;
using YeahTVApiLibrary.Manager;
using YeahTVApiLibrary.Service;
using YeahTVApiLibrary.Service.Cache;
using YeahTVApiLibrary.Service.HotelMemberInfoManager;
using YeahTVApiLibrary.WrapperFacade;
using YeahTVLibrary.Manager;
using YeahWebApi.MongoRepository;

namespace YeahTVApi.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            container.LoadConfiguration();
            container.AddNewExtension<Interception>();

            RegisterRepertorys(container);
            RegisterServices(container);
            RegisterManagers(container);
            RegisterApi(container);

            // set all interface has logging interception behavior 
            container.Registrations
                .ToList()
                .ForEach(r =>
                {
                    container.RegisterType(r.RegisteredType,
                        new Interceptor<InterfaceInterceptor>(),
                        new InterceptionBehavior<UnitOfWorkInterceptionBehavior>());
                });
        }

        private static void RegisterApi(IUnityContainer container)
        {
            container.RegisterType<IHttpContextService, HttpContextService>();
        }

        private static void RegisterManagers(IUnityContainer container)
        {
            container.RegisterType<ICacheManager, CacheManager>();
            container.RegisterType<IRedisCacheManager, RedisCacheManager>();
            container.RegisterType<ITraceManager, TraceMagager>();
            container.RegisterType<IAppLibraryManager, AppLibraryManager>();
            container.RegisterType<IDeviceAppsMonitorManager, DeviceAppsMonitorManager>();
            container.RegisterType<ISystemConfigManager, SystemConfigManager>();
            container.RegisterType<IHotelMovieTraceManager, HotelMovieTraceManager>();
            container.RegisterType<IHotelTVChannelManager, HotelTVChannelManager>();
            container.RegisterType<IMovieTemplateRelationManager, MovieTemplateRelationManager>();
            container.RegisterType<ISysAttachmentManager, SysAttachmentManager>();
            container.RegisterType<ITVChannelManager, TVChannelManager>();
            container.RegisterType<IDeviceTraceLibraryManager, DeviceTraceManager>();
            container.RegisterType<IFileUploadServiceManager, FileUploadServiceManager>();
            container.RegisterType<IBackupDeviceManager, BackupDeviceManager>();
            container.RegisterType<ITVHotelConfigManager, TVHotelConfigManager>();
            container.RegisterType<ILogManager, LogManager>();
            container.RegisterType<IConstantSystemConfigManager, ConstantSystemConfigManager>();
            container.RegisterType<IQiniuCloudManager, QiniuCloudManager>();
            container.RegisterType<IVODOrderManager, VODOrderManager>();
            container.RegisterType<IVODPaymentResultManager, VODPaymentResultManager>();
            container.RegisterType<IMovieManager, MovieManager>();
            container.RegisterType<IMovieTemplateManager, MovieTemplateManager>();
            container.RegisterType<IHotelMovieTraceNoTemplateManager, HotelMovieTraceNoTemplateManager>();
            container.RegisterType<IHotelMovieTraceNoTemplateWrapperFacade, HotelMovieTraceNoTemplateWrapperFacade>();
            container.RegisterType<IMovieForLocalizeManager, MovieForLocalizeManager>();
            container.RegisterType<ITagManager, TagManager>();
            container.RegisterType<ILocalizeResourceManager, LocalizeResourceManager>();
            container.RegisterType<IHCSTaskManager, HCSTaskManager>();
            container.RegisterType<IHCSGlobalConfigManager, HCSGlobalConfigManager>();
            container.RegisterType<IStoreOrderManager, StoreOrderManager>();

            container.RegisterType<IHotelTypeFactory, HotelTypeFactory>();
            container.RegisterType<IGlobalConfigManager, GlobalConfigManager>();
            container.RegisterType<IOpenApiManager, OpenApiManager>();
            container.RegisterType<IScoreExchangManager, ScoreExchangManager>();
            container.RegisterType<IOrderQRCodeRecordManager, OrderQRCodeRecordManager>();
        }

        private static void RegisterServices(IUnityContainer container)
        {
            container.RegisterType<IAlarmClockService, AlarmClockService>();
            container.RegisterType<IGetGuestInfoService, GetGuestInfoService>();
            container.RegisterType<IHotelListService, HotelListService>();
            container.RegisterType<IMemberInfoService, MemberInfoService>();
            container.RegisterType<IPriceFutureService, PriceFutureService>();
            container.RegisterType<IPriceService, PriceService>();
            container.RegisterType<IRegisterMemberService, RegisterMemberService>();
            container.RegisterType<ISelfServiceService, SelfServiceService>();
            container.RegisterType<IAppToolService, AppToolService>();
            container.RegisterType<ICheckInService, CheckInService>();
            container.RegisterType<IRedisCacheService, RedisCacheService>();
            container.RegisterType<IRoomManagerService, RoomManagerService>();
            container.RegisterType<IHotelCommodityService, HotelCommodityService>();
            container.RegisterType<IHotelCommonService, HotelCommonService>();
            container.RegisterType<IRequestApiService, RequestApiService>();
            container.RegisterType<IImageUpdateServiceProvider, ImageUpdateServiceProvider>();
            container.RegisterType<IQiniuCloudService, QiniuCloudService>();
            container.RegisterType<IVODOrderRepertory, VODOrderRepertory>();
            container.RegisterType<IWeiXinService, WeiXinService>();
        }

        private static void RegisterRepertorys(IUnityContainer container)
        {
            container.RegisterType<ITVTraceRepertory, TVTraceRepertory>();
            container.RegisterType<ITVHotelConfigRepertory, TVHotelConfigRepertory>();
            container.RegisterType<ITVAppsRepertory, TVAppsRepertory>();
            container.RegisterType<ITVAppVersionRepertory, TVAppVersionRepertory>();
            container.RegisterType<IAppPublishLibraryRepertory, AppPublishRepertory>();
            container.RegisterType<ISystemConfigRepertory, SystemConfigRepertory>();
            container.RegisterType<ISystemLogRepertory, SystemLogRepertory>();
            container.RegisterType<IBehaviorLogRepertory, BehaviorLogRepertory>();
            container.RegisterType<IDeviceAppsMonitorRepertory, DeviceAppsMonitorRepertory>();
            container.RegisterType<IHotelMovieTraceRepertory, HotelMovieTraceRepertory>();
            container.RegisterType<IHotelTVChannelRepertory, HotelTVChannelRepertory>();
            container.RegisterType<IAppVersionLibraryRepertory, AppVersionRepertory>();
            container.RegisterType<IMovieTemplateRelationRepertory, MovieTemplateRelationRepertory>();
            container.RegisterType<ISysAttachmentRepertory, SysAttachmentRepertory>();
            container.RegisterType<ITVChannelRepertory, TVChannelRepertory>();
            container.RegisterType<IDeviceTraceLibraryRepertory, DeviceTraceRepertory>();
            container.RegisterType<IAppsLibraryRepertory, AppsRepertory>();
            container.RegisterType<IBackupDeviceRepertory, BackupDeviceRepertory>();
            container.RegisterType<IMovieTemplateRepertory, MovieTemplateRepertory>();
            container.RegisterType<IVODPaymentResultRepertory, VODPaymentResultRepertory>();
            container.RegisterType<IMovieRepertory, MovieRepertory>();
            container.RegisterType<IMongoLogRepository, MongoLogRepository>();
            container.RegisterType<IHotelMovieTraceNoTemplateRepertory, HotelMovieTraceNoTemplateRepertory>();
            container.RegisterType<IMovieForLocalizeRepertory, MovieForLocalizeRepertory>();
            container.RegisterType<ITagRepertory, TagRepertory>();
            container.RegisterType<ILocalizeResourceRepertory, LocalizeResourceRepertory>();
            container.RegisterType<IHCSTaskRepertory, HCSTaskRepertory>();
            container.RegisterType<IHCSConfigRepertory, HCSConfigRepertory>();
            container.RegisterType<IHCSJobRepertory, HCSJobRepertory>();
            container.RegisterType<IMongoDeviceTraceRepository, MongoDeviceTraceRepository>();
            container.RegisterType<IStoreOrderRepertory, StoreOrderRepertory>();
            container.RegisterType<IGlobalConfigRepertory, GlobalConfigRepertory>();
            container.RegisterType<IScoreExchangRepertory, ScoreExchangRepertory>();
            container.RegisterType<IOrderQRCodeRecordRepertory, OrderQRCodeRecordRepertory>();
        }

        private static void SetLocatorProvider(IUnityContainer container)
        {
            var locator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => locator);
        }

    }
}
