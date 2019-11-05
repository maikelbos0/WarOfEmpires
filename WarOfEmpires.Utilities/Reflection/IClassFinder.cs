using System;
using System.Collections.Generic;

namespace WarOfEmpires.Utilities.Reflection {
    public interface IClassFinder {
        IEnumerable<Type> FindAllClasses();
    }
}