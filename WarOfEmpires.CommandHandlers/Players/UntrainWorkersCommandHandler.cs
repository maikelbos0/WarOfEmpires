using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Players {
    [InterfaceInjectable]
    [Audit]
    public sealed class UntrainWorkersCommandHandler : ICommandHandler<UntrainWorkersCommand> {
        public CommandResult<UntrainWorkersCommand> Execute(UntrainWorkersCommand command) {
            throw new System.NotImplementedException();
        }
    }
}