using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Repositories.Markets;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.CommandHandlers.Markets {
    [InterfaceInjectable]
    [Audit]
    public sealed class BuyResourcesCommandHandler : ICommandHandler<BuyResourcesCommand> {
        private readonly CaravanRepository _repository;
        private readonly EnumFormatter _formatter;

        public BuyResourcesCommandHandler(CaravanRepository repository, EnumFormatter formatter) {
            _repository = repository;
            _formatter = formatter;
        }

        public CommandResult<BuyResourcesCommand> Execute(BuyResourcesCommand command) {
            throw new System.NotImplementedException();
        }
    }
}