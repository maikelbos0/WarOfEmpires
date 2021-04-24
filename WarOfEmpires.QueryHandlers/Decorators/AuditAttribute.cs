using System;
using VDT.Core.DependencyInjection.Decorators;

namespace WarOfEmpires.QueryHandlers.Decorators {
    //[AttributeUsage(AttributeTargets.Method)]
    public sealed class AuditAttribute : Attribute, IDecorateAttribute<AuditDecorator> { }
}