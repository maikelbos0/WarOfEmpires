using System;
using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Events;
using WarOfEmpires.Domain.Events;
using WarOfEmpires.Repositories.Events;
using WarOfEmpires.Utilities.Events;

namespace WarOfEmpires.CommandHandlers.Events {
    [TransientServiceImplementation(typeof(ICommandHandler<RunScheduledTasksCommand>))]
    public sealed class RunScheduledTasksCommandHandler : ICommandHandler<RunScheduledTasksCommand> {
        private readonly IScheduledTaskRepository _repository;
        private readonly IEventService _eventService;

        public RunScheduledTasksCommandHandler(IScheduledTaskRepository repository, IEventService eventService) {
            _repository = repository;
            _eventService = eventService;
        }

        [Audit]
        public CommandResult<RunScheduledTasksCommand> Execute(RunScheduledTasksCommand command) {
            var result = new CommandResult<RunScheduledTasksCommand>();

            foreach (var task in _repository.GetAll()) {
                while (task.Execute()) {
                    _eventService.Dispatch((IEvent)Activator.CreateInstance(Type.GetType(task.EventType)));
                    _repository.SaveChanges();
                }
            }

            return result;
        }
    }
}