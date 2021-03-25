using System.Linq;

using Unity.AspNet.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WarOfEmpires.UnityMvcActivator), nameof(WarOfEmpires.UnityMvcActivator.Start))]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(WarOfEmpires.UnityMvcActivator), nameof(WarOfEmpires.UnityMvcActivator.Shutdown))]

namespace WarOfEmpires {
    /// <summary>
    /// Provides the bootstrapping for integrating Unity with ASP.NET MVC.
    /// </summary>
    public static class UnityMvcActivator {
        /// <summary>
        /// Integrates Unity when the application starts.
        /// </summary>
        public static void Start() {
            //TODO move to .NET Core DI

            //FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            //FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(UnityConfig.Container));

            //DependencyResolver.SetResolver(new UnityDependencyResolver(UnityConfig.Container));
        }

        /// <summary>
        /// Disposes the Unity container when the application is shut down.
        /// </summary>
        public static void Shutdown() {
            UnityConfig.Container.Dispose();
        }
    }
}