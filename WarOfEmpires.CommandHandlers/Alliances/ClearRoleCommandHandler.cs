using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class ClearRoleCommandHandler : ICommandHandler<ClearRoleCommand> {
        private readonly IAllianceRepository _repository;

        public ClearRoleCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        public CommandResult<ClearRoleCommand> Execute(ClearRoleCommand command) {
            var result = new CommandResult<ClearRoleCommand>();
            var alliance = _repository.Get(command.Email);
            var member = alliance.Members.Single(p => p.Id == int.Parse(command.PlayerId));

            alliance.ClearRole(member);
            _repository.SaveChanges();

            return result;
        }
    }
}