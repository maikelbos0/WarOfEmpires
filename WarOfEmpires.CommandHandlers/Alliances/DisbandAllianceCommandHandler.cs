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
            var result = new CommandResult<DisbandAllianceCommand>();
            var alliance = _repository.Get(command.Email);

            alliance.Disband();
            _repository.Remove(alliance);

            return result;
        }
    }
}
