using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class AcceptNonAggressionPactRequestCommandHandler : ICommandHandler<AcceptNonAggressionPactRequestCommand> {
        private readonly IAllianceRepository _repository;

        public AcceptNonAggressionPactRequestCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        public CommandResult<AcceptNonAggressionPactRequestCommand> Execute(AcceptNonAggressionPactRequestCommand command) {
            throw new System.NotImplementedException();
        }
    }
}
