using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.Practices.Unity;
using Unity.WebApi;
using YeahHCSTVApi;
using System.Web.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(YeahHCSTVApi.UnityWebActivator), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(YeahHCSTVApi.UnityWebActivator), "Shutdown")]

namespace YeahHCSTVApi
{
    /// <summary>Provides the bootstrapping for integrating Unity with ASP.NET MVC.</summary>
    public static class UnityWebActivator
    {
        /// <summary>Integrates Unity when the application starts.</summary>
        public static void Start()
        {
            RegisterFilterProviders();
        }

        /// <summary>Disposes the Unity container when the application is shut down.</summary>
        public static void Shutdown()
        {
            var container = UnityConfig.GetConfiguredContainer();
            container.Dispose();
        }

        private static void RegisterFilterProviders()
        {
            var providers =
                GlobalConfiguration.Configuration.Services.GetFilterProviders().ToList();

            GlobalConfiguration.Configuration.Services.Add(
                typeof(System.Web.Http.Filters.IFilterProvider),
                new UnityActionFilterProvider(UnityConfig.GetConfiguredContainer()));

            var defaultprovider = providers.First(p => p is ActionDescriptorFilterProvider);

            GlobalConfiguration.Configuration.Services.Remove(
                typeof(System.Web.Http.Filters.IFilterProvider),
                defaultprovider);
        }
    }
}