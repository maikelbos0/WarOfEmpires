using WarOfEmpires.Commands;
using WarOfEmpires.Domain.Auditing;
using WarOfEmpires.Repositories.Auditing;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Serialization;

namespace WarOfEmpires.CommandHandlers.Decorators {
    public sealed class AuditDecorator<TCommand> : Decorator<ICommandHandler<TCommand>>, ICommandHandler<TCommand> where TCommand : ICommand {
        private readonly ICommandExecutionRepository _repository;
        private readonly ISerializer _serializer;

        public AuditDecorator(ICommandExecutionRepository repository, ISerializer serializer) {
            _repository = repository;
            _serializer = serializer;
        }

        public CommandResult<TCommand> Execute(TCommand command) {
            _repository.Add(new CommandExecution(typeof(TCommand).FullName, _serializer.SerializeToJson(command)));

            return Handler.Execute(command);
        }
    }
}