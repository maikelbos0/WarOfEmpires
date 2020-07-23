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
            throw new System.NotImplementedException();
        }
    }
}