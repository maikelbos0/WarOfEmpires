using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class DissolveNonAggressionPactCommandHandler : ICommandHandler<DissolveNonAggressionPactCommand> {
        private readonly IAllianceRepository _repository;

        public DissolveNonAggressionPactCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        public CommandResult<DissolveNonAggressionPactCommand> Execute(DissolveNonAggressionPactCommand command) {
            throw new System.NotImplementedException();
        }
    }
}
