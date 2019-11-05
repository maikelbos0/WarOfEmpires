using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Decorators {
    public sealed class AuditAttribute : DecoratorAttribute {
        public AuditAttribute() : base(typeof(AuditDecorator<,>)) {
        }
    }
}