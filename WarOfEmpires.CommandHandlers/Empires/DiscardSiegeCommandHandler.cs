using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class DiscardSiegeCommandHandler : ICommandHandler<DiscardSiegeCommand> {
        public PlayerRepository _repository;

        public DiscardSiegeCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<DiscardSiegeCommand> Execute(DiscardSiegeCommand command) {
            throw new System.NotImplementedException();
        }
    }
}