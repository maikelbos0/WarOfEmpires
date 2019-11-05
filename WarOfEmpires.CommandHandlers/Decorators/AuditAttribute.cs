using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Decorators {
    public sealed class AuditAttribute : DecoratorAttribute {
        public AuditAttribute() : base(typeof(AuditDecorator<>)) {
        }
    }
}