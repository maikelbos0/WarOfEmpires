using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [TransientServiceImplementation(typeof(ICommandHandler<ClearRoleCommand>))]
    public sealed class ClearRoleCommandHandler : ICommandHandler<ClearRoleCommand> {
        private readonly IAllianceRepository _repository;

        public ClearRoleCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<ClearRoleCommand> Execute(ClearRoleCommand command) {
            var result = new CommandResult<ClearRoleCommand>();
            var alliance = _repository.Get(command.Email);
            var member = alliance.Members.Single(p => p.Id == command.PlayerId);

            alliance.ClearRole(member);
            _repository.SaveChanges();

            return result;
        }
    }
}