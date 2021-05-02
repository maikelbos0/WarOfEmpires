using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VDT.Core.DependencyInjection;

namespace WarOfEmpires.Utilities.DependencyInjection {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddServices(this IServiceCollection services, Assembly entryAssembly) {
            foreach (var assembly in FindAssemblies(entryAssembly, assemblyName => assemblyName.FullName.Contains("WarOfEmpires"))) {
                services.AddAttributeServices(assembly, options => options.AddAttributeDecorators());
            }

            return services;
        }

        private static IEnumerable<Assembly> FindAssemblies(Assembly assembly, Func<AssemblyName, bool> predicate) {
            var assemblies = new HashSet<Assembly>();

            if (predicate(assembly.GetName())) {
                assemblies.Add(assembly);

                foreach (var referencedAssembly in assembly.GetReferencedAssemblies().Where(predicate).Select(Assembly.Load)) {
                    foreach (var foundAssembly in FindAssemblies(referencedAssembly, predicate)) {
                        assemblies.Add(foundAssembly);
                    }
                }
            }

            return assemblies;
        }
    }
}
