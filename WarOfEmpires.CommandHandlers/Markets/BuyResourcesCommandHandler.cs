using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.CommandHandlers.Markets {
    [InterfaceInjectable]
    [Audit]
    public sealed class BuyResourcesCommandHandler : ICommandHandler<BuyResourcesCommand> {
        private readonly PlayerRepository _repository;
        private readonly EnumFormatter _formatter;

        public BuyResourcesCommandHandler(PlayerRepository repository, EnumFormatter formatter) {
            _repository = repository;
            _formatter = formatter;
        }

        public CommandResult<BuyResourcesCommand> Execute(BuyResourcesCommand command) {
            throw new System.NotImplementedException();
        }
    }
}