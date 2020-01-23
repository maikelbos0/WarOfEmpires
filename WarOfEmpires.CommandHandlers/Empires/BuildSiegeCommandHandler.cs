using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class BuildSiegeCommandHandler : ICommandHandler<BuildSiegeCommand> {
        public PlayerRepository _repository;

        public BuildSiegeCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<BuildSiegeCommand> Execute(BuildSiegeCommand command) {
            throw new System.NotImplementedException();
        }
    }
}