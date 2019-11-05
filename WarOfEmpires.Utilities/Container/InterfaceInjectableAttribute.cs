using System;

namespace WarOfEmpires.Utilities.Container {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class InterfaceInjectableAttribute : Attribute { }
}