using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand> {
        private readonly IPlayerRepository _repository;

        public CreateRoleCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<CreateRoleCommand> Execute(CreateRoleCommand command) {
            var result = new CommandResult<CreateRoleCommand>();
            var player = _repository.Get(command.Email);

            player.Alliance.CreateRole(command.Name);

            _repository.Update();

            return result;
        }
    }
}