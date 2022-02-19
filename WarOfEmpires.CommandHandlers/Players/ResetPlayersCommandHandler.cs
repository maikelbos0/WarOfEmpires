using VDT.Core.DependencyInjection.Attributes;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Players;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Players {
    [TransientServiceImplementation(typeof(ICommandHandler<ResetPlayersCommand>))]
    public sealed class ResetPlayersCommandHandler : ICommandHandler<ResetPlayersCommand> {
        private readonly IPlayerRepository _repository;
        private readonly IAllianceRepository _allianceRepository;

        public ResetPlayersCommandHandler(IPlayerRepository repository, IAllianceRepository allianceRepository) {
            _repository = repository;
            _allianceRepository = allianceRepository;
        }

        [Audit]
        public CommandResult<ResetPlayersCommand> Execute(ResetPlayersCommand command) {
            var result = new CommandResult<ResetPlayersCommand>();

            foreach (var player in _repository.GetAll()) {
                player.Reset();
            }

            foreach (var alliance in _allianceRepository.GetAll()) {
                alliance.Reset();
            }

            _repository.SaveChanges();
            
            return result;
        }
    }
}
