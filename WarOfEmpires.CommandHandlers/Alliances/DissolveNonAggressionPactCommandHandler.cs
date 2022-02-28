using System.Linq;
using WarOfEmpires.Utilities.Auditing;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;

namespace WarOfEmpires.CommandHandlers.Alliances {
    public sealed class DissolveNonAggressionPactCommandHandler : ICommandHandler<DissolveNonAggressionPactCommand> {
        private readonly IAllianceRepository _repository;

        public DissolveNonAggressionPactCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<DissolveNonAggressionPactCommand> Execute(DissolveNonAggressionPactCommand command) {
            var result = new CommandResult<DissolveNonAggressionPactCommand>();
            var alliance = _repository.Get(command.Email);
            var pact = alliance.NonAggressionPacts.Single(p => p.Id == command.NonAggressionPactId);

            pact.Dissolve();
            _repository.SaveChanges();

            return result;
        }
    }
}
