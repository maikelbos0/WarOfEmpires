using System;
using System.Linq;
using WarOfEmpires.Utilities.Auditing;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;

namespace WarOfEmpires.CommandHandlers.Alliances {
    public sealed class TransferLeadershipCommandHandler : ICommandHandler<TransferLeadershipCommand> {
        private readonly IAllianceRepository _repository;

        public TransferLeadershipCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<TransferLeadershipCommand> Execute(TransferLeadershipCommand command) {
            var result = new CommandResult<TransferLeadershipCommand>();
            var alliance = _repository.Get(command.Email);
            var newLeader = alliance.Members.Single(p => p.Id == command.MemberId);

            if (alliance.Leader == newLeader) {
                throw new InvalidOperationException("You can't transfer leadership to yourself");
            }

            alliance.TransferLeadership(newLeader);
            _repository.SaveChanges();

            return result;
        }
    }
}
