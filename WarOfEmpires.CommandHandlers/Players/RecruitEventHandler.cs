using WarOfEmpires.Commands.Players;
using WarOfEmpires.Domain.Events;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.CommandHandlers.Players {
    public sealed class RecruitEventHandler : IEventHandler<RecruitEvent> {
        private readonly RecruitCommandHandler _recruitCommandHandler;

        public RecruitEventHandler(RecruitCommandHandler recruitCommandHandler) {
            _recruitCommandHandler = recruitCommandHandler;
        }

        public void Handle(RecruitEvent domainEvent) {
            _recruitCommandHandler.Execute(new RecruitCommand());
        }
    }
}