using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class TransferLeadershipCommandHandler : ICommandHandler<TransferLeadershipCommand> {
        private readonly IAllianceRepository _repository;

        public TransferLeadershipCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        public CommandResult<TransferLeadershipCommand> Execute(TransferLeadershipCommand command) {
            throw new System.NotImplementedException();
        }
    }
}
