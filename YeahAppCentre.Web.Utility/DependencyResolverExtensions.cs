using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Mvc;

namespace System.Web.Api
{
    public static class DependencyResolverExtensions
    {

        // Summary:
        //     Resolves singly registered services that support arbitrary object creation.
        //
        // Parameters:
        //   resolver:
        //     The dependency resolver instance that this method extends.
        //
        // Type parameters:
        //   TService:
        //     The type of the requested service or object.
        //
        // Returns:
        //     The requested service or object.
        public static TService GetService<TService>(this System.Web.Http.Dependencies.IDependencyResolver resolver) where TService : class
        {
            return resolver.BeginScope().GetService(typeof(TService)) as TService;
        }

        //
        // Summary:
        //     Resolves multiply registered services.
        //
        // Parameters:
        //   resolver:
        //     The dependency resolver instance that this method extends.
        //
        // Type parameters:
        //   TService:
        //     The type of the requested services.
        //
        // Returns:
        //     The requested services.
        public static IEnumerable<TService> GetServices<TService>(this System.Web.Http.Dependencies.IDependencyResolver resolver) where TService : class
        {
            return resolver.BeginScope().GetServices(typeof(TService)) as IEnumerable<TService>;
        }
    }    
}