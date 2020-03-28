﻿using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Events;
using WarOfEmpires.Repositories.Events;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Events {
    [InterfaceInjectable]
    [Audit]
    public sealed class PauseScheduledTasksCommandHandler : ICommandHandler<PauseScheduledTasksCommand> {
        private readonly IScheduledTaskRepository _repository;

        public PauseScheduledTasksCommandHandler(IScheduledTaskRepository repository) {
            _repository = repository;
        }

        public CommandResult<PauseScheduledTasksCommand> Execute(PauseScheduledTasksCommand command) {
            var result = new CommandResult<PauseScheduledTasksCommand>();

            foreach (var task in _repository.GetAll()) {
                if (!task.IsPaused) {
                    task.Pause();
                }
            }

            _repository.Update();

            return result;
        }
    }
}