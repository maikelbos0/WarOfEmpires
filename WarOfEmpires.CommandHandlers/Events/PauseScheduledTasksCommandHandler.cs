﻿using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Events;
using WarOfEmpires.Repositories.Events;

namespace WarOfEmpires.CommandHandlers.Events {
    [TransientServiceImplementation(typeof(ICommandHandler<PauseScheduledTasksCommand>))]
    public sealed class PauseScheduledTasksCommandHandler : ICommandHandler<PauseScheduledTasksCommand> {
        private readonly IScheduledTaskRepository _repository;

        public PauseScheduledTasksCommandHandler(IScheduledTaskRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<PauseScheduledTasksCommand> Execute(PauseScheduledTasksCommand command) {
            var result = new CommandResult<PauseScheduledTasksCommand>();

            foreach (var task in _repository.GetAll()) {
                if (!task.IsPaused) {
                    task.Pause();
                }
            }

            _repository.SaveChanges();

            return result;
        }
    }
}