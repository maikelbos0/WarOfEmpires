using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class RejectNonAggressionPactRequestCommandHandler : ICommandHandler<RejectNonAggressionPactRequestCommand> {
        private readonly IAllianceRepository _repository;

        public RejectNonAggressionPactRequestCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        public CommandResult<RejectNonAggressionPactRequestCommand> Execute(RejectNonAggressionPactRequestCommand command) {
            throw new System.NotImplementedException();
        }
    }
}
