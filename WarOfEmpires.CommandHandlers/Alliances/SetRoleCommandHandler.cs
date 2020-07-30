using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class SetRoleCommandHandler : ICommandHandler<SetRoleCommand> {
        private readonly IPlayerRepository _repository;

        public SetRoleCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<SetRoleCommand> Execute(SetRoleCommand command) {
            var result = new CommandResult<SetRoleCommand>();
            var player = _repository.Get(command.Email);
            var member = player.Alliance.Members.Single(p => p.Id == int.Parse(command.PlayerId));
            var role = player.Alliance.Roles.Single(r => r.Id == int.Parse(command.RoleId));

            player.Alliance.SetRole(member, role);

            _repository.Update();

            return result;
        }
    }
}