using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Players;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Players {
    [InterfaceInjectable]
    [Audit]
    public sealed class TrainWorkersCommandHandler : ICommandHandler<TrainWorkersCommand> {
        public PlayerRepository _repository;

        public TrainWorkersCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<TrainWorkersCommand> Execute(TrainWorkersCommand command) {
            throw new System.NotImplementedException();
        }
    }
}