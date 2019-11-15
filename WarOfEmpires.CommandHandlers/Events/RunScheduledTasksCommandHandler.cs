using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Events;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Events {
    [InterfaceInjectable]
    [Audit]
    public sealed class RunScheduledTasksCommandHandler : ICommandHandler<RunScheduledTasksCommand> {
        public CommandResult<RunScheduledTasksCommand> Execute(RunScheduledTasksCommand command) {
            throw new System.NotImplementedException();
        }
    }
}