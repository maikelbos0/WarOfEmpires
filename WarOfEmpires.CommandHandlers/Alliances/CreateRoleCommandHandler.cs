using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;

namespace WarOfEmpires.CommandHandlers.Alliances {
    public sealed class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand> {
        private readonly IAllianceRepository _repository;

        public CreateRoleCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<CreateRoleCommand> Execute(CreateRoleCommand command) {
            var result = new CommandResult<CreateRoleCommand>();
            var alliance = _repository.Get(command.Email);

            alliance.CreateRole(command.Name, command.CanInvite, command.CanManageRoles, command.CanDeleteChatMessages, command.CanKickMembers, command.CanManageNonAggressionPacts, command.CanManageWars, command.CanBank);
            _repository.SaveChanges();

            return result;
        }
    }
}