using System;
using System.Linq;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;

namespace WarOfEmpires.CommandHandlers.Alliances {
    public sealed class SendNonAggressionPactRequestCommandHandler : ICommandHandler<SendNonAggressionPactRequestCommand> {
        private readonly IAllianceRepository _repository;

        public SendNonAggressionPactRequestCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        public CommandResult<SendNonAggressionPactRequestCommand> Execute(SendNonAggressionPactRequestCommand command) {
            var result = new CommandResult<SendNonAggressionPactRequestCommand>();
            var sender = _repository.Get(command.Email);
            var recipient = _repository.Get(command.AllianceId);
            
            if (sender == recipient) {
                throw new InvalidOperationException("You can't send a non-aggression pact request to yourself");
            }

            if (recipient.ReceivedNonAggressionPactRequests.Any(r => r.Sender == sender)) {
                result.AddError("This alliance already has an outstanding non-aggression pact request from your alliance");
            }

            if (recipient.NonAggressionPacts.Any(r => r.Alliances.Any(a => a == sender))) {
                result.AddError("Your alliance is already in a non-aggression pact with this alliance");
            }

            if (result.Success) {
                sender.SendNonAggressionPactRequest(recipient);
                _repository.SaveChanges();
            }

            return result;
        }
    }
}
