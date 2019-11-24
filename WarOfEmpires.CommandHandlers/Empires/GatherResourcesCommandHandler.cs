using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class GatherResourcesCommandHandler : ICommandHandler<GatherResourcesCommand> {
        public PlayerRepository _repository;

        public GatherResourcesCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<GatherResourcesCommand> Execute(GatherResourcesCommand command) {
            var result = new CommandResult<GatherResourcesCommand>();

            foreach (var player in _repository.GetAll()) {
                player.GatherResources();
            }

            _repository.Update();

            return result;
        }
    }
}