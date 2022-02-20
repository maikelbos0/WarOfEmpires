using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;

namespace WarOfEmpires.CommandHandlers.Alliances {
    public sealed class WithdrawInviteCommandHandler : ICommandHandler<WithdrawInviteCommand> {
        private readonly IAllianceRepository _repository;

        public WithdrawInviteCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<WithdrawInviteCommand> Execute(WithdrawInviteCommand command) {
            var result = new CommandResult<WithdrawInviteCommand>();
            var alliance = _repository.Get(command.Email);
            var invite = alliance.Invites.Single(i => i.Id == command.InviteId);

            alliance.RemoveInvite(invite);
            _repository.SaveChanges();

            return result;
        }
    }
}