using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class SendAllianceInviteCommandHandler : ICommandHandler<SendAllianceInviteCommand> {
        private readonly IPlayerRepository _repository;

        public SendAllianceInviteCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<SendAllianceInviteCommand> Execute(SendAllianceInviteCommand command) {
            throw new System.NotImplementedException();
        }
    }
}
