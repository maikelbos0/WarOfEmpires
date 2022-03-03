using System.Linq;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;

namespace WarOfEmpires.CommandHandlers.Alliances {
    public sealed class WithdrawNonAggressionPactRequestCommandHandler : ICommandHandler<WithdrawNonAggressionPactRequestCommand> {
        private readonly IAllianceRepository _repository;

        public WithdrawNonAggressionPactRequestCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        public CommandResult<WithdrawNonAggressionPactRequestCommand> Execute(WithdrawNonAggressionPactRequestCommand command) {
            var result = new CommandResult<WithdrawNonAggressionPactRequestCommand>();
            var alliance = _repository.Get(command.Email);
            var request = alliance.SentNonAggressionPactRequests.Single(r => r.Id == command.NonAggressionPactRequestId);

            request.Withdraw();
            _repository.SaveChanges();

            return result;
        }
    }
}
