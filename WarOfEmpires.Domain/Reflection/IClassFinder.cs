using System;
using System.Collections.Generic;

namespace WarOfEmpires.Domain.Reflection {
    public interface IClassFinder {
        IEnumerable<Type> FindAllClasses();
    }
}