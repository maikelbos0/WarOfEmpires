using VDT.Core.DependencyInjection.Attributes;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Utilities.Events;

namespace WarOfEmpires.CommandHandlers.Empires {
    [TransientServiceImplementation(typeof(IEventHandler<TurnTaskTriggeredEvent>))]
    public sealed class TurnTaskTriggeredEventHandler : IEventHandler<TurnTaskTriggeredEvent> {
        private readonly ICommandHandler<ProcessTurnCommand> _commandHandler;

        public TurnTaskTriggeredEventHandler(ICommandHandler<ProcessTurnCommand> commandHandler) {
            _commandHandler = commandHandler;
        }

        public void Handle(TurnTaskTriggeredEvent domainEvent) {
            _commandHandler.Execute(new ProcessTurnCommand());
        }
    }
}