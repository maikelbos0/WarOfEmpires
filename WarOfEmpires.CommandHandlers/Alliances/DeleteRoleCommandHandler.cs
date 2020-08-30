using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand> {
        private readonly IAllianceRepository _repository;

        public DeleteRoleCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        public CommandResult<DeleteRoleCommand> Execute(DeleteRoleCommand command) {
            var result = new CommandResult<DeleteRoleCommand>();
            var alliance = _repository.Get(command.Email);
            var role = alliance.Roles.Single(r => r.Id == int.Parse(command.RoleId));

            alliance.DeleteRole(role);
            _repository.SaveChanges();

            return result;
        }
    }
}