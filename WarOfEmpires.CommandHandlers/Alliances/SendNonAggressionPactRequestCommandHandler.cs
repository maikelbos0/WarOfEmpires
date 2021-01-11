using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class SendNonAggressionPactRequestCommandHandler : ICommandHandler<SendNonAggressionPactRequestCommand> {
        private readonly IAllianceRepository _repository;

        public SendNonAggressionPactRequestCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        public CommandResult<SendNonAggressionPactRequestCommand> Execute(SendNonAggressionPactRequestCommand command) {
            throw new System.NotImplementedException();
        }
    }
}
