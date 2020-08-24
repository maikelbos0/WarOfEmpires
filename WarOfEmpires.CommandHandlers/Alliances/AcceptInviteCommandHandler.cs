using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class AcceptInviteCommandHandler : ICommandHandler<AcceptInviteCommand> {
        private readonly IPlayerRepository _playerRepository;
        private readonly IAllianceRepository _allianceRepository;

        public AcceptInviteCommandHandler(IPlayerRepository playerRepository, IAllianceRepository allianceRepository) {
            _playerRepository = playerRepository;
            _allianceRepository = allianceRepository;
        }

        public CommandResult<AcceptInviteCommand> Execute(AcceptInviteCommand command) {
            var result = new CommandResult<AcceptInviteCommand>();
            var player = _playerRepository.Get(command.Email);
            var invite = player.Invites.Single(i => i.Id == int.Parse(command.InviteId));

            if (player.Alliance != null) {
                result.AddError("You are already in an alliance; leave your current alliance before accepting an invite");
            }

            if (result.Success) {
                invite.Alliance.AddMember(player);
                _allianceRepository.RemoveInvite(invite);
            }

            return result;
        }
    }
}