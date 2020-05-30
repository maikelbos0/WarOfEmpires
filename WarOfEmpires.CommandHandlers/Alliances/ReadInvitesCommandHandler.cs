using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class ReadInvitesCommandHandler : ICommandHandler<ReadInvitesCommand> {
        private readonly IPlayerRepository _repository;

        public ReadInvitesCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<ReadInvitesCommand> Execute(ReadInvitesCommand command) {
            var result = new CommandResult<ReadInvitesCommand>();
            var player = _repository.Get(command.Email);

            if (player.Invites.Any(i => !i.IsRead)) {
                foreach (var invite in player.Invites.Where(i => !i.IsRead)) {
                    invite.IsRead = true;
                }

                _repository.Update();
            }

            return result;
        }
    }
}