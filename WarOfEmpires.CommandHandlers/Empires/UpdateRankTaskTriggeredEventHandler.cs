using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Utilities.Events;

namespace WarOfEmpires.CommandHandlers.Empires {
    public sealed class UpdateRankTaskTriggeredEventHandler : IEventHandler<UpdateRankTaskTriggeredEvent> {
        private readonly ICommandHandler<UpdateRankCommand> _commandHandler;

        public UpdateRankTaskTriggeredEventHandler(ICommandHandler<UpdateRankCommand> commandHandler) {
            _commandHandler = commandHandler;
        }

        public void Handle(UpdateRankTaskTriggeredEvent domainEvent) {
            _commandHandler.Execute(new UpdateRankCommand());
        }
    }
}