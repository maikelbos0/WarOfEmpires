using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Events;
using WarOfEmpires.Repositories.Events;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Events {
    [InterfaceInjectable]
    [Audit]
    public sealed class PauseScheduledTasksCommandHandler : ICommandHandler<PauseScheduledTasksCommand> {
        private readonly ScheduledTaskRepository _repository;

        public PauseScheduledTasksCommandHandler(ScheduledTaskRepository repository) {
            _repository = repository;
        }

        public CommandResult<PauseScheduledTasksCommand> Execute(PauseScheduledTasksCommand command) {
            var result = new CommandResult<PauseScheduledTasksCommand>();

            foreach (var task in _repository.GetAll()) {
                task.Pause();
            }

            _repository.Update();

            return result;
        }
    }
}