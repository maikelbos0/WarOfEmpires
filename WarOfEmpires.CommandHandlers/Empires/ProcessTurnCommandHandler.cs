using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class ProcessTurnCommandHandler : ICommandHandler<ProcessTurnCommand> {
        private readonly IPlayerRepository _repository;

        public ProcessTurnCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<ProcessTurnCommand> Execute(ProcessTurnCommand command) {
            var result = new CommandResult<ProcessTurnCommand>();

            foreach (var player in _repository.GetAll()) {
                player.ProcessTurn();
            }

            _repository.SaveChanges();

            return result;
        }
    }
}