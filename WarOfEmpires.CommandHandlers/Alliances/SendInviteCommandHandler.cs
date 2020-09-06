using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class SendInviteCommandHandler : ICommandHandler<SendInviteCommand> {
        private readonly IPlayerRepository _repository;

        public SendInviteCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<SendInviteCommand> Execute(SendInviteCommand command) {
            var result = new CommandResult<SendInviteCommand>();
            var player = _repository.Get(int.Parse(command.PlayerId));
            var member = _repository.Get(command.Email);
            var alliance = member.Alliance;

            if (alliance.Invites.Any(i => i.Player == player)) {
                result.AddError("This player already has an open invite and can not be invited again");
            }

            if (result.Success) {
                // TODO create domain function and rewrite tests
                alliance.Invites.Add(new Invite(alliance, player, command.Subject, command.Body));
                _repository.SaveChanges();
            }

            return result;
        }
    }
}
