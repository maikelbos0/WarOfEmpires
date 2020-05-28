using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class RejectInviteCommandHandler : ICommandHandler<RejectInviteCommand> {
        private readonly IPlayerRepository _repository;

        public RejectInviteCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<RejectInviteCommand> Execute(RejectInviteCommand command) {
            throw new System.NotImplementedException();
        }
    }
}