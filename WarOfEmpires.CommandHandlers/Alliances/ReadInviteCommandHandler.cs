using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class ReadInviteCommandHandler : ICommandHandler<ReadInviteCommand> {
        private readonly IPlayerRepository _repository;

        public ReadInviteCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<ReadInviteCommand> Execute(ReadInviteCommand command) {
            var result = new CommandResult<ReadInviteCommand>();
            var player = _repository.Get(command.Email);
            var inviteId = int.Parse(command.InviteId);
            var invite = player.Invites.Single(m => m.Id == inviteId);

            invite.IsRead = true;

            _repository.SaveChanges();

            return result;
        }
    }
}