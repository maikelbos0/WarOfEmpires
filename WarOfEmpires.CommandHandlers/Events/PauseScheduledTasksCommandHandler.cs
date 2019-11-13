using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Events;
using WarOfEmpires.Repositories.Events;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Events {
    [InterfaceInjectable]
    [Audit]
    public sealed class PauseScheduledTasksCommandHandler : ICommandHandler<UnpauseScheduledTasksCommand> {
        private readonly ScheduledTaskRepository _repository;

        public PauseScheduledTasksCommandHandler(ScheduledTaskRepository repository) {
            _repository = repository;
        }

        public CommandResult<UnpauseScheduledTasksCommand> Execute(UnpauseScheduledTasksCommand command) {
            throw new System.NotImplementedException();
        }
    }
}