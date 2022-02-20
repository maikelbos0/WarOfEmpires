using System;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Empires {
    public sealed class PrioritizeResearchCommandHandler : ICommandHandler<PrioritizeResearchCommand> {
        private readonly IPlayerRepository _repository;

        public PrioritizeResearchCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<PrioritizeResearchCommand> Execute(PrioritizeResearchCommand command) {
            var result = new CommandResult<PrioritizeResearchCommand>();
            var player = _repository.Get(command.Email);
            var researchType = (ResearchType)Enum.Parse(typeof(ResearchType), command.ResearchType);

            player.PrioritizeResearch(researchType);
            _repository.SaveChanges();

            return result;
        }
    }
}
