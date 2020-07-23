using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand> {
        private readonly IPlayerRepository _repository;

        public DeleteRoleCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<DeleteRoleCommand> Execute(DeleteRoleCommand command) {
            throw new System.NotImplementedException();
        }
    }
}