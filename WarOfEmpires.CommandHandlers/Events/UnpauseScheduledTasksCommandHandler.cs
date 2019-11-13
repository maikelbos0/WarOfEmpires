﻿using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Events;
using WarOfEmpires.Repositories.Events;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Events {
    [InterfaceInjectable]
    [Audit]
    public sealed class UnpauseScheduledTasksCommandHandler : ICommandHandler<UnpauseScheduledTasksCommand> {
        private readonly ScheduledTaskRepository _repository;

        public UnpauseScheduledTasksCommandHandler(ScheduledTaskRepository repository) {
            _repository = repository;
        }

        public CommandResult<UnpauseScheduledTasksCommand> Execute(UnpauseScheduledTasksCommand command) {
            var result = new CommandResult<UnpauseScheduledTasksCommand>();

            foreach (var task in _repository.GetAll()) {
                task.Unpause();
            }

            _repository.Update();

            return result;
        }
    }
}