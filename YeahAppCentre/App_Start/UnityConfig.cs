using System;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using YeahCentre.EntityFrameworkRepository;
using YeahCentre.EntityFrameworkRepository.Repertory;
using YeahCentre.Manager;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.Infrastructure.RepositoriesInterface.EntityFrameworkRepositoryInterface.IRepertory.YeahCentre;
using YeahCenter.Infrastructure;
using YeahTVApiLibrary.Behavior;
using YeahTVApiLibrary.EntityFrameworkRepository;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Manager;
using YeahTVApiLibrary.Service.Cache;
using YeahTVApi.Manager;
using YeahTVApiLibrary;
using YeahTVLibrary.Manager;
using YeahTVApi.ServiceProvider;
using YeahTVApiLibrary.Infrastructure.ManagerInterface;
using YeahTVApiLibrary.Service;
using YeahWebApi.MongoRepository;
using YeahTVApiLibrary.WrapperFacade;
using YeahTVApiLibrary.Infrastructure.RepositoriesInterface.IRepertory;
using YeahCenter.Infrastructure.WrapperFacadeInterface;
using YeahCentre.WrapperFacade;

namespace YeahAppCentre
{
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
                      new InterceptionBehavior<YeahCentreWebUnitOfWorkInterceptionBehavior>()
                      );
              });
        }
    }

    public class YeahCentreWebUnitOfWorkInterceptionBehavior : UnitOfWorkInterceptionBehaviorBase
    {
        protected override EFUnitOfWork CreateUnitOfWork()
        {
            return new EFUnitOfWork(new YeahCentreContext(Constant.NameOrConnectionString));
        }
    }
}