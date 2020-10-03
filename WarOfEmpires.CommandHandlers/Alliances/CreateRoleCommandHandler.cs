using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand> {
        private readonly IAllianceRepository _repository;

        public CreateRoleCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        public CommandResult<CreateRoleCommand> Execute(CreateRoleCommand command) {
            var result = new CommandResult<CreateRoleCommand>();
            var alliance = _repository.Get(command.Email);

            alliance.CreateRole(command.Name, command.CanInvite, command.CanManageRoles);
            _repository.SaveChanges();

            return result;
        }
    }
}