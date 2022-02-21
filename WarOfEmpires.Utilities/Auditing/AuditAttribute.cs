using System;
using VDT.Core.DependencyInjection.Decorators;

namespace WarOfEmpires.Utilities.Auditing {
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class AuditAttribute : Attribute, IDecorateAttribute<IAuditDecorator> { }
}