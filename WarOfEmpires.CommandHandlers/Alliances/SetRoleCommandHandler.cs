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
            throw new System.NotImplementedException();
        }
    }
}