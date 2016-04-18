using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Web.Http;
using Unity.WebApi;
using YeahTVApi.DomainModel;
using YeahTVApiLibrary.Behavior;
using YeahTVApiLibrary.EntityFrameworkRepository;
using YeahTVApiLibrary.EntityFrameworkRepository.Models;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Manager;
using YeahTVLibrary.Manager;
using System.Linq;
using YeahTVApi.EntityFrameworkRepository.Models;

namespace YeahHCSTVApi
{
    public static class UnityConfig
    {
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            container.AddNewExtension<Interception>();

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
            container.RegisterTypes(
              AllClasses.FromLoadedAssemblies().Where(
                t => t.Name.Contains("Repertory") || t.Name.Contains("Manager") || t.Name.Contains("Service") || t.Name.Contains("Repository") || t.Name.Contains("Facade")),
              WithMappings.FromMatchingInterface);
            
            container.RegisterType<IDeviceTraceLibraryRepertory, DeviceTraceRepertory>();
            container.RegisterType<IAppsLibraryRepertory, AppsRepertory>();
            container.RegisterType<IAppPublishLibraryRepertory, AppPublishRepertory>();
            container.RegisterType<IAppVersionLibraryRepertory, AppVersionRepertory>();

            container.RegisterType<IAppLibraryManager, AppLibraryManager>();
            container.RegisterType<IDeviceTraceLibraryManager, DeviceTraceManager>();

            container.Registrations
              .ToList()
              .ForEach(r =>
              {
                  container.RegisterType(r.RegisteredType,
                      new Interceptor<InterfaceInterceptor>(),
                      new InterceptionBehavior<YeahTvHcsApiUnitOfWorkInterceptionBehavior>()
                      );
              });
        }
    }

    public class YeahTvHcsApiUnitOfWorkInterceptionBehavior : UnitOfWorkInterceptionBehaviorBase
    {
        protected override EFUnitOfWork CreateUnitOfWork()
        {
            return new EFUnitOfWork(new YeahTVContext(Constant.NameOrConnectionString));
        }
    }
}