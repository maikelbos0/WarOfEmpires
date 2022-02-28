using System.Linq;
using WarOfEmpires.Utilities.Auditing;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Empires {
    public sealed class RemoveQueuedResearchCommandHandler : ICommandHandler<RemoveQueuedResearchCommand> {
        private readonly IPlayerRepository _repository;

        public RemoveQueuedResearchCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<RemoveQueuedResearchCommand> Execute(RemoveQueuedResearchCommand command) {
            var result = new CommandResult<RemoveQueuedResearchCommand>();
            var player = _repository.Get(command.Email);
            var queuedResearch = player.QueuedResearch.Single(r => r.Id == command.QueudResearchId);

            player.RemoveQueuedResearch(queuedResearch);
            _repository.SaveChanges();

            return result;
        }
    }
}
