using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class ProcessTurnCommandHandler : ICommandHandler<ProcessTurnCommand> {
        private readonly PlayerRepository _repository;

        public ProcessTurnCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<ProcessTurnCommand> Execute(ProcessTurnCommand command) {
            var result = new CommandResult<ProcessTurnCommand>();

            foreach (var player in _repository.GetAll()) {
                player.ProcessTurn();
            }

            _repository.Update();

            return result;
        }
    }
}