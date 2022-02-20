using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;

namespace WarOfEmpires.CommandHandlers.Alliances {
    public sealed class AcceptNonAggressionPactRequestCommandHandler : ICommandHandler<AcceptNonAggressionPactRequestCommand> {
        private readonly IAllianceRepository _repository;

        public AcceptNonAggressionPactRequestCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<AcceptNonAggressionPactRequestCommand> Execute(AcceptNonAggressionPactRequestCommand command) {
            var result = new CommandResult<AcceptNonAggressionPactRequestCommand>();
            var alliance = _repository.Get(command.Email);
            var request = alliance.ReceivedNonAggressionPactRequests.Single(r => r.Id == command.NonAggressionPactRequestId);

            if (alliance.NonAggressionPacts.Any(p => p.Alliances.Contains(request.Sender))) {
                result.AddError("You are already in a non-aggression pact with this alliance.");
            }

            if (result.Success) {
                request.Accept();
                _repository.SaveChanges();
            }

            return result;
        }
    }
}
