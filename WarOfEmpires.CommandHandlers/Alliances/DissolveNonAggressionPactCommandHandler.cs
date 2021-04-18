using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [ScopedServiceImplementation(typeof(ICommandHandler<DissolveNonAggressionPactCommand>))]
    [Audit]
    public sealed class DissolveNonAggressionPactCommandHandler : ICommandHandler<DissolveNonAggressionPactCommand> {
        private readonly IAllianceRepository _repository;

        public DissolveNonAggressionPactCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

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
