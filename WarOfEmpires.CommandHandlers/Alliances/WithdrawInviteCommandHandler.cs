using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class WithdrawInviteCommandHandler : ICommandHandler<WithdrawInviteCommand> {
        private readonly IPlayerRepository _repository;

        public WithdrawInviteCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<WithdrawInviteCommand> Execute(WithdrawInviteCommand command) {
            var result = new CommandResult<WithdrawInviteCommand>();
            var member = _repository.Get(command.Email);
            var alliance = member.Alliance;
            var invite = alliance.Invites.Single(i => i.Id == int.Parse(command.InviteId));

            _repository.RemoveInvite(invite);

            return result;
        }
    }
}