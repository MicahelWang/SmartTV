using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;
using System.Linq;
using YeahTVApiLibrary.Behavior;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary;
using YeahTVApiLibrary.EntityFrameworkRepository;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;
using YeahTVApiLibrary.Manager;
using YeahTVLibrary.Manager;
using YeahTVApiLibrary.Service;

namespace YeahOnlieShoppingMall.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();

            RegisterComponents(container);

            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }

        public static void RegisterComponents(IUnityContainer container)
        {
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.AddNewExtension<Interception>();

            container.RegisterType<IDeviceTraceLibraryRepertory, DeviceTraceRepertory>();
            container.RegisterType<IAppsLibraryRepertory, AppsRepertory>();
            container.RegisterType<IAppPublishLibraryRepertory, AppPublishRepertory>();
            container.RegisterType<IAppVersionLibraryRepertory, AppVersionRepertory>();

            container.RegisterType<IAppLibraryManager, AppLibraryManager>();
            container.RegisterType<IDeviceTraceLibraryManager, DeviceTraceManager>();

            container.RegisterTypes(
              AllClasses.FromLoadedAssemblies().Where(
                t => t.Name.Contains("Repertory") || t.Name.Contains("Manager") || t.Name.Contains("Service") || t.Name.Contains("Repository") || t.Name.Contains("Facade")),
              WithMappings.FromMatchingInterface);

            container.Registrations
              .ToList()
              .ForEach(r =>
              {
                  container.RegisterType(r.RegisteredType,
                      new Interceptor<InterfaceInterceptor>(),
                      new InterceptionBehavior<UnitOfWorkInterceptionBehaviorBase>()
                      );
              });
        }
    }

}
