using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Utilities.Events;

namespace WarOfEmpires.CommandHandlers.Empires {
    public sealed class TurnTaskTriggeredEventHandler : IEventHandler<TurnTaskTriggeredEvent> {
        private readonly ProcessTurnCommandHandler _processTurnCommandHandler;

        public TurnTaskTriggeredEventHandler(ProcessTurnCommandHandler processTurnCommandHandler) {
            _processTurnCommandHandler = processTurnCommandHandler;
        }

        public void Handle(TurnTaskTriggeredEvent domainEvent) {
            _processTurnCommandHandler.Execute(new ProcessTurnCommand());
        }
    }
}