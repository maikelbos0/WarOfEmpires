using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Players;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Players {
    [InterfaceInjectable]
    [Audit]
    public sealed class UntrainWorkersCommandHandler : ICommandHandler<UntrainWorkersCommand> {
        public PlayerRepository _repository;

        public UntrainWorkersCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<UntrainWorkersCommand> Execute(UntrainWorkersCommand command) {
            throw new System.NotImplementedException();
        }
    }
}