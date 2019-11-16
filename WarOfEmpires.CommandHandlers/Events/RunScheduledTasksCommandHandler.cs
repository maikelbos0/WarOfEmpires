using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Events;
using WarOfEmpires.Repositories.Events;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Events {
    [InterfaceInjectable]
    [Audit]
    public sealed class RunScheduledTasksCommandHandler : ICommandHandler<RunScheduledTasksCommand> {
        private readonly ScheduledTaskRepository _repository;

        public RunScheduledTasksCommandHandler(ScheduledTaskRepository repository) {
            _repository = repository;
        }

        public CommandResult<RunScheduledTasksCommand> Execute(RunScheduledTasksCommand command) {
            var result = new CommandResult<RunScheduledTasksCommand>();

            foreach (var task in _repository.GetAll()) {
                while (task.Execute()) { }
            }

            _repository.Update();

            return result;
        }
    }
}