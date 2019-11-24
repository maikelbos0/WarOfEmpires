using WarOfEmpires.Commands.Players;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Utilities.Events;

namespace WarOfEmpires.CommandHandlers.Players {
    public sealed class RecruitTaskTriggeredEventHandler : IEventHandler<RecruitTaskTriggeredEvent> {
        private readonly RecruitCommandHandler _recruitCommandHandler;

        public RecruitTaskTriggeredEventHandler(RecruitCommandHandler recruitCommandHandler) {
            _recruitCommandHandler = recruitCommandHandler;
        }

        public void Handle(RecruitTaskTriggeredEvent domainEvent) {
            _recruitCommandHandler.Execute(new RecruitCommand());
        }
    }
}