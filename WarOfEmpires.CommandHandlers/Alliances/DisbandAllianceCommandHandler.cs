using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class DisbandAllianceCommandHandler : ICommandHandler<DisbandAllianceCommand> {
        private readonly IAllianceRepository _repository;

        public DisbandAllianceCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        public CommandResult<DisbandAllianceCommand> Execute(DisbandAllianceCommand command) {
            throw new System.NotImplementedException();
        }
    }
}
