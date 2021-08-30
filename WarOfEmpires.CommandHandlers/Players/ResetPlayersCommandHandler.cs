using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Players;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Players {
    [TransientServiceImplementation(typeof(ICommandHandler<ResetPlayersCommand>))]
    public sealed class ResetPlayersCommandHandler : ICommandHandler<ResetPlayersCommand> {
        private readonly IPlayerRepository _repository;

        public ResetPlayersCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<ResetPlayersCommand> Execute(ResetPlayersCommand command) {
            var result = new CommandResult<ResetPlayersCommand>();

            foreach (var player in _repository.GetAll()) {
                player.Reset();
            }

            _repository.SaveChanges();
            
            return result;
        }
    }
}
