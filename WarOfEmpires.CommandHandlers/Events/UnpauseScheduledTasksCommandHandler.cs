using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Events;
using WarOfEmpires.Repositories.Events;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Events {
    [InterfaceInjectable]
    [Audit]
    public sealed class UnpauseScheduledTasksCommandHandler : ICommandHandler<UnpauseScheduledTasksCommand> {
        private readonly IScheduledTaskRepository _repository;

        public UnpauseScheduledTasksCommandHandler(IScheduledTaskRepository repository) {
            _repository = repository;
        }

        public CommandResult<UnpauseScheduledTasksCommand> Execute(UnpauseScheduledTasksCommand command) {
            var result = new CommandResult<UnpauseScheduledTasksCommand>();

            foreach (var task in _repository.GetAll()) {
                if (task.IsPaused) {
                    task.Unpause();
                }
            }

            _repository.Update();

            return result;
        }
    }
}