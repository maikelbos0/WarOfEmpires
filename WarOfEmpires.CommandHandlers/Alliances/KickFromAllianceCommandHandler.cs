using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class KickFromAllianceCommandHandler : ICommandHandler<KickFromAllianceCommand> {
        private readonly IAllianceRepository _repository;

        public KickFromAllianceCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        public CommandResult<KickFromAllianceCommand> Execute(KickFromAllianceCommand command) {
            throw new System.NotImplementedException();
        }
    }
}