using VDT.Core.DependencyInjection.Attributes;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Events;
using WarOfEmpires.Repositories.Events;

namespace WarOfEmpires.CommandHandlers.Events {
    [TransientServiceImplementation(typeof(ICommandHandler<UnpauseScheduledTasksCommand>))]
    public sealed class UnpauseScheduledTasksCommandHandler : ICommandHandler<UnpauseScheduledTasksCommand> {
        private readonly IScheduledTaskRepository _repository;

        public UnpauseScheduledTasksCommandHandler(IScheduledTaskRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<UnpauseScheduledTasksCommand> Execute(UnpauseScheduledTasksCommand command) {
            var result = new CommandResult<UnpauseScheduledTasksCommand>();

            foreach (var task in _repository.GetAll()) {
                if (task.IsPaused) {
                    task.Unpause();
                }
            }

            _repository.SaveChanges();

            return result;
        }
    }
}