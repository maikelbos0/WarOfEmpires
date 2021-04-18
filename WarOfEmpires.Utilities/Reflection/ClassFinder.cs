using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VDT.Core.DependencyInjection;

namespace WarOfEmpires.Utilities.Reflection {
    // TODO remove FindAllClasses
    // TODO rename to AssemblyFinder

    [ScopedServiceImplementation(typeof(IClassFinder))]
    public sealed class ClassFinder : IClassFinder {
        public IEnumerable<Assembly> FindAllAssemblies() {
            return FindAssemblies(Assembly.GetEntryAssembly(), assemblyName => assemblyName.FullName.Contains("WarOfEmpires"));
        }

        private IEnumerable<Assembly> FindAssemblies(Assembly assembly, Func<AssemblyName, bool> predicate) {
            var assemblies = new HashSet<Assembly>();

            if (predicate(assembly.GetName())) {
                assemblies.Add(assembly);
            }

            foreach (var referencedAssembly in assembly.GetReferencedAssemblies().Where(predicate).Select(Assembly.Load)) {
                foreach (var foundAssembly in FindAssemblies(referencedAssembly, predicate)) {
                    assemblies.Add(foundAssembly);
                }
            }

            return assemblies;
        }

        public IEnumerable<Type> FindAllClasses() {
            return FindAllAssemblies()
                .SelectMany(assembly => assembly.GetTypes());
        }
    }
}