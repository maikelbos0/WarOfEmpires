using System;
using System.Collections.Generic;
using System.Reflection;

namespace WarOfEmpires.Utilities.Reflection {
    public interface IClassFinder {
        IEnumerable<Assembly> FindAllAssemblies();
        IEnumerable<Type> FindAllClasses();
    }
}