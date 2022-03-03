using System.Linq;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;

namespace WarOfEmpires.CommandHandlers.Alliances {
    public sealed class SetRoleCommandHandler : ICommandHandler<SetRoleCommand> {
        private readonly IAllianceRepository _repository;

        public SetRoleCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        public CommandResult<SetRoleCommand> Execute(SetRoleCommand command) {
            var result = new CommandResult<SetRoleCommand>();
            var alliance = _repository.Get(command.Email);
            var member = alliance.Members.Single(p => p.Id == command.PlayerId);
            var role = alliance.Roles.Single(r => r.Id == command.RoleId);

            alliance.SetRole(member, role);
            _repository.SaveChanges();

            return result;
        }
    }
}