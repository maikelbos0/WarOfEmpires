using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class RejectInviteCommandHandler : ICommandHandler<RejectInviteCommand> {
        private readonly IPlayerRepository _playerRepository;
        private readonly IAllianceRepository _allianceRepository;

        public RejectInviteCommandHandler(IPlayerRepository playerRepository, IAllianceRepository allianceRepository) {
            _playerRepository = playerRepository;
            _allianceRepository = allianceRepository;
        }

        public CommandResult<RejectInviteCommand> Execute(RejectInviteCommand command) {
            var result = new CommandResult<RejectInviteCommand>();
            var player = _playerRepository.Get(command.Email);
            var invite = player.Invites.Single(i => i.Id == int.Parse(command.InviteId));

            invite.Alliance.RemoveInvite(invite);
            _allianceRepository.Update();

            return result;
        }
    }
}