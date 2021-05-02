using System.Diagnostics;
using VDT.Core.DependencyInjection;
using VDT.Core.DependencyInjection.Decorators;
using WarOfEmpires.Domain.Auditing;
using WarOfEmpires.Repositories.Auditing;
using WarOfEmpires.Utilities.Serialization;

namespace WarOfEmpires.CommandHandlers.Decorators {
    [ScopedServiceImplementation(typeof(IAuditDecorator))]
    public sealed class AuditDecorator : IAuditDecorator {
        private readonly ICommandExecutionRepository _repository;
        private readonly ISerializer _serializer;
        private Stopwatch _stopwatch;

        public AuditDecorator(ICommandExecutionRepository repository, ISerializer serializer) {
            _repository = repository;
            _serializer = serializer;
        }

        void IDecorator.BeforeExecute(MethodExecutionContext context) {
            _stopwatch = Stopwatch.StartNew();
        }

        void IDecorator.AfterExecute(MethodExecutionContext context) {
            _stopwatch.Stop();

            if (context.Arguments.Length > 0 && context.Arguments[0] != null) {
                _repository.Add(new CommandExecution(context.Arguments[0].GetType().FullName, _serializer.SerializeToJson(context.Arguments[0]), _stopwatch.Elapsed.TotalMilliseconds));
            }
        }
    }
}