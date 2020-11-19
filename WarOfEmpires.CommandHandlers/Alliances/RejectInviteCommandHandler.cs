using System.Linq;
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
            var result = new CommandResult<RejectInviteCommand>();
            var player = _repository.Get(command.Email);
            var invite = player.Invites.Single(i => i.Id == command.InviteId);

            invite.Alliance.RemoveInvite(invite);
            _repository.SaveChanges();

            return result;
        }
    }
}