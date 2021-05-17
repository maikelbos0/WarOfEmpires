using VDT.Core.DependencyInjection;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Utilities.Events;

namespace WarOfEmpires.CommandHandlers.Empires {
    [TransientServiceImplementation(typeof(IEventHandler<RecruitTaskTriggeredEvent>))]
    public sealed class RecruitTaskTriggeredEventHandler : IEventHandler<RecruitTaskTriggeredEvent> {
        private readonly ICommandHandler<RecruitCommand> _commandHandler;

        public RecruitTaskTriggeredEventHandler(ICommandHandler<RecruitCommand> commandHandler) {
            _commandHandler = commandHandler;
        }

        public void Handle(RecruitTaskTriggeredEvent domainEvent) {
            _commandHandler.Execute(new RecruitCommand());
        }
    }
}