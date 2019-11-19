using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Reflection;
using System;
using Unity;

namespace WarOfEmpires {
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig {
        #region Unity Container
        private readonly static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() => {
              var service = new ContainerService(new ClassFinder());
              
              return service.GetContainer();
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion
    }
}