using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Domain.Alliances;
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
            var result = new CommandResult<SendAllianceInviteCommand>();
            var player = _repository.Get(int.Parse(command.PlayerId));
            var member = _repository.Get(command.Email);
            var alliance = member.Alliance;

            if (alliance.Invites.Any(i => i.Player == player)) {
                result.AddError("This player already has an open invite and can not be invited again");
            }

            if (result.Success) {
                alliance.Invites.Add(new Invite(alliance, player, command.Message));
                _repository.Update();
            }

            return result;
        }
    }
}
