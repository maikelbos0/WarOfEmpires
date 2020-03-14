using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Markets {
    [InterfaceInjectable]
    [Audit]
    public sealed class SellResourcesCommandHandler : ICommandHandler<SellResourcesCommand> {
        private readonly PlayerRepository _repository;

        public SellResourcesCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<SellResourcesCommand> Execute(SellResourcesCommand command) {
            throw new System.NotImplementedException();
        }
    }
}