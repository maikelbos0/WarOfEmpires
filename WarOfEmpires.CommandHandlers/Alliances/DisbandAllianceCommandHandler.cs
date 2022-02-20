using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;

namespace WarOfEmpires.CommandHandlers.Alliances {
    public sealed class DisbandAllianceCommandHandler : ICommandHandler<DisbandAllianceCommand> {
        private readonly IAllianceRepository _repository;

        public DisbandAllianceCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<DisbandAllianceCommand> Execute(DisbandAllianceCommand command) {
            var result = new CommandResult<DisbandAllianceCommand>();
            var alliance = _repository.Get(command.Email);

            alliance.Disband();
            _repository.Remove(alliance);

            return result;
        }
    }
}
