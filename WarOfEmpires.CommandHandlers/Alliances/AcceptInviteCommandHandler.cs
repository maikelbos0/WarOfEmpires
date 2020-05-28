using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class AcceptInviteCommandHandler : ICommandHandler<AcceptInviteCommand> {
        private readonly IPlayerRepository _repository;

        public AcceptInviteCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<AcceptInviteCommand> Execute(AcceptInviteCommand command) {
            throw new System.NotImplementedException();
        }
    }
}