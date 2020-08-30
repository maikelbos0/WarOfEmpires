using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class WithdrawInviteCommandHandler : ICommandHandler<WithdrawInviteCommand> {
        private readonly IAllianceRepository _repository;

        public WithdrawInviteCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        public CommandResult<WithdrawInviteCommand> Execute(WithdrawInviteCommand command) {
            var result = new CommandResult<WithdrawInviteCommand>();
            var alliance = _repository.Get(command.Email);
            var invite = alliance.Invites.Single(i => i.Id == int.Parse(command.InviteId));

            alliance.RemoveInvite(invite);
            _repository.Update();

            return result;
        }
    }
}