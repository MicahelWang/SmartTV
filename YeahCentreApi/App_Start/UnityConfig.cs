using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System.Web.Http;
using Unity.WebApi;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Manager;
using YeahTVApiLibrary.Service;
using System.Linq;
using YeahTVApiLibrary.Behavior;
using System;
using YeahCentre.EntityFrameworkRepository;
using YeahCentre.EntityFrameworkRepository.Repertory;
using YeahCentre.Manager;
using YeahTVApi.DomainModel;
using YeahTVApi.Infrastructure;
using YeahCenter.Infrastructure;
using YeahTVApiLibrary.EntityFrameworkRepository;
using YeahTVApiLibrary.Service.Cache;
using YeahWebApi.MongoRepository;
using IHotelManager = YeahCenter.Infrastructure.IHotelManager;
using YeahTVApiLibrary.WrapperFacade;
using YeahTVLibrary.Manager;
using YeahTVApiLibrary;
using YeahTVApi.Manager;
using YeahTVApiLibrary.Infrastructure.ManagerInterface;
using YeahTVApi.Infrastructure.RepositoriesInterface.EntityFrameworkRepositoryInterface.IRepertory.YeahCentre;
using YeahTVApiLibrary.Infrastructure.RepositoriesInterface.IRepertory;

namespace YeahCentreApi
{
    public static class UnityConfig
    {
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
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
    }

    public class YeahCentreUnitOfWorkInterceptionBehavior : UnitOfWorkInterceptionBehaviorBase
    {
        protected override EFUnitOfWork CreateUnitOfWork()
        {
            return new EFUnitOfWork(new YeahCentreContext(Constant.NameOrConnectionString));
        }
    }
}