using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class ClearRoleCommandHandler : ICommandHandler<ClearRoleCommand> {
        private readonly IPlayerRepository _repository;

        public ClearRoleCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<ClearRoleCommand> Execute(ClearRoleCommand command) {
            var result = new CommandResult<ClearRoleCommand>();
            var player = _repository.Get(command.Email);
            var member = player.Alliance.Members.Single(p => p.Id == int.Parse(command.PlayerId));

            player.Alliance.ClearRole(member);

            _repository.Update();

            return result;
        }
    }
}