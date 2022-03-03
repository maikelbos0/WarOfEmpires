using System;
using WarOfEmpires.Commands.Game;
using WarOfEmpires.Domain.Game;
using WarOfEmpires.Repositories.Game;

namespace WarOfEmpires.CommandHandlers.Game {
    public sealed class SetGamePhaseCommandHandler : ICommandHandler<SetGamePhaseCommand> {
        private readonly IGameStatusRepository _repository;

        public SetGamePhaseCommandHandler(IGameStatusRepository repository) {
            _repository = repository;
        }

        public CommandResult<SetGamePhaseCommand> Execute(SetGamePhaseCommand command) {
            var result = new CommandResult<SetGamePhaseCommand>();
            var phase = (GamePhase)Enum.Parse(typeof(GamePhase), command.Phase);
            var gameStatus = _repository.Get();

            gameStatus.Phase = phase;
            _repository.SaveChanges();

            return result;
        }
    }
}
