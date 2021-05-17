using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [TransientServiceImplementation(typeof(ICommandHandler<WithdrawNonAggressionPactRequestCommand>))]
    public sealed class WithdrawNonAggressionPactRequestCommandHandler : ICommandHandler<WithdrawNonAggressionPactRequestCommand> {
        private readonly IAllianceRepository _repository;

        public WithdrawNonAggressionPactRequestCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        [Audit]
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
