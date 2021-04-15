using System;

namespace WarOfEmpires.Utilities.Container {
    [Obsolete]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class InterfaceInjectableAttribute : Attribute { }
}