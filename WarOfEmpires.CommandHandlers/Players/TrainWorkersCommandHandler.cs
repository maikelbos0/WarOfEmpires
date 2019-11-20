using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Players {
    [InterfaceInjectable]
    [Audit]
    public sealed class TrainWorkersCommandHandler : ICommandHandler<TrainWorkersCommand> {
        public CommandResult<TrainWorkersCommand> Execute(TrainWorkersCommand command) {
            throw new System.NotImplementedException();
        }
    }
}