using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.Utilities.Reflection {
    // TODO probably doesn't need injection and interface
    [InterfaceInjectable]
    public sealed class ClassFinder : IClassFinder {
        // TODO rename to AssemblyFinder

        public IEnumerable<Assembly> FindAllAssemblies() {
            // Ensure that all present solution assemblies are loaded
            foreach (var assemblyFile in Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "*WarOfEmpires*.dll")) {
                Assembly.LoadFile(assemblyFile);
            }

            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.FullName.Contains("WarOfEmpires"));
        }

        public IEnumerable<Type> FindAllClasses() {
            return FindAllAssemblies()
                .SelectMany(assembly => assembly.GetTypes());
        }
    }
}