using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class WithdrawInviteCommandHandler : ICommandHandler<WithdrawInviteCommand> {
        private readonly IPlayerRepository _playerRepository;
        private readonly IAllianceRepository _allianceRepository;

        public WithdrawInviteCommandHandler(IPlayerRepository playerRepository, IAllianceRepository allianceRepository) {
            _playerRepository = playerRepository;
            _allianceRepository = allianceRepository;
        }

        public CommandResult<WithdrawInviteCommand> Execute(WithdrawInviteCommand command) {
            var result = new CommandResult<WithdrawInviteCommand>();
            var member = _playerRepository.Get(command.Email);
            var alliance = member.Alliance;
            var invite = alliance.Invites.Single(i => i.Id == int.Parse(command.InviteId));

            _allianceRepository.RemoveInvite(invite);

            return result;
        }
    }
}