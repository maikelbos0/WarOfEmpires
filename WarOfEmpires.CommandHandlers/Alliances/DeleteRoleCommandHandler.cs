using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [ScopedServiceImplementation(typeof(ICommandHandler<DeleteRoleCommand>))]
    public sealed class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand> {
        private readonly IAllianceRepository _repository;

        public DeleteRoleCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<DeleteRoleCommand> Execute(DeleteRoleCommand command) {
            var result = new CommandResult<DeleteRoleCommand>();
            var alliance = _repository.Get(command.Email);
            var role = alliance.Roles.Single(r => r.Id == command.RoleId);

            alliance.DeleteRole(role);
            _repository.SaveChanges();

            return result;
        }
    }
}