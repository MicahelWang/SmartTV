using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Linq;
using YeahCenter.Infrastructure;
using YeahCentre.EntityFrameworkRepository;
using YeahCentre.EntityFrameworkRepository.Repertory;
using YeahCentre.Manager;
using YeahTVApi.DomainModel;
using YeahTVApiLibrary.Behavior;
using YeahTVApiLibrary.EntityFrameworkRepository;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Manager;
using YeahTVApiLibrary.Service.Cache;
using YeahWebApi.MongoRepository;

namespace OpenApi
{
    public static class UnityConfig
    {
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
       {
           var container = new UnityContainer();
           container.AddNewExtension<Interception>();

           RegisterComponents(container);
           RegisterRepertorys(container);
           RegisterManagers(container);

           container.Registrations
            .ToList()
            .ForEach(r =>
            {
                container.RegisterType(r.RegisteredType,
                    new Interceptor<InterfaceInterceptor>(),
                    new InterceptionBehavior<YeahCentreUnitOfWorkInterceptionBehavior>()
                    );
            });
           return container;
       });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }

        public static void RegisterRepertorys(IUnityContainer container)
        {
            container.RegisterType<IAuthCertigierManagerManager, AuthCertigierManagerManager>();
            container.RegisterType<IUserManager, UserManager>();
            container.RegisterType<IRoleManager, RoleManager>();


            container.RegisterType<IAuthTVTokenManager, AuthTVTokenManager>();
            container.RegisterType<ILogManager, LogManager>();
            container.RegisterType<IConstantSystemConfigManager, ConstantSystemConfigManager>();
            
        }
        public static void RegisterManagers(IUnityContainer container)
        {
            
            container.RegisterType<IAuthCertigierManagerRepertory, AuthCertigierManagerRepertory>();
            container.RegisterType<IAuthUserDeviceTraceRepertory, AuthUserDeviceTraceRepertory>();
            container.RegisterType<ISysUserRepertory, SysUserRepertory>();
            container.RegisterType<ISysLoginRepertory, SysLoginRepertory>();
            
            container.RegisterType<ISysRoleResourceRelationRepertory, SysRoleResourceRelationRepertory>();
            container.RegisterType<ISysRoleRepertory, SysRoleRepertory>();
            container.RegisterType<IPowerResourceRepertory, PowerResourceRepertory>();
            container.RegisterType<ISystemConfigRepertory, SystemConfigRepertory>();
           
             
            



            container.RegisterType<IAuthTVTokenRepertory, AuthTVTokenRepertory>();
            container.RegisterType<IBehaviorLogRepertory, BehaviorLogRepertory>();
            container.RegisterType<ISystemLogRepertory, SystemLogRepertory>();
            container.RegisterType<IMongoLogRepository, MongoLogRepository>();
            
            
            
        }

        public static void RegisterComponents(IUnityContainer container)
        {
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IAppLibraryManager, AppLibraryManager>();

            container.RegisterType<IDeviceTraceLibraryRepertory, DeviceTraceRepertory>();
            container.RegisterType<IAppsLibraryRepertory, AppsRepertory>();


            container.RegisterType<IRedisCacheManager, RedisCacheManager>();
            container.RegisterType<IRedisCacheService, RedisCacheService>();
            
        }
    }

    public class YeahCentreUnitOfWorkInterceptionBehavior : UnitOfWorkInterceptionBehaviorBase
    {
        protected override EFUnitOfWork CreateUnitOfWork()
        {
            return new EFUnitOfWork(new YeahCentreContext(Constant.NameOrConnectionString));
        }
    }

}