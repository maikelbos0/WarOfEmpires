using WarOfEmpires.Domain.Auditing;
using WarOfEmpires.Utilities.Serialization;
using WarOfEmpires.Database;
using System.Diagnostics;
using VDT.Core.DependencyInjection.Decorators;
using VDT.Core.DependencyInjection.Attributes;

namespace WarOfEmpires.QueryHandlers.Decorators {
    [TransientServiceImplementation(typeof(IAuditDecorator))]
    public sealed class AuditDecorator : IAuditDecorator {
        private readonly IWarContext _context;
        private readonly ISerializer _serializer;
        private Stopwatch _stopwatch;

        public AuditDecorator(IWarContext context, ISerializer serializer) {
            _context = context;
            _serializer = serializer;
        }

        void IDecorator.BeforeExecute(MethodExecutionContext context) {
            _stopwatch = Stopwatch.StartNew();
        }

        void IDecorator.AfterExecute(MethodExecutionContext context) {
            _stopwatch.Stop();

            if (context.Arguments.Length > 0 && context.Arguments[0] != null) {
                // Explicitly use the context to prevent having to use a repository
                _context.QueryExecutions.Add(new QueryExecution(context.Arguments[0].GetType().FullName, _serializer.SerializeToJson(context.Arguments[0]), _stopwatch.Elapsed.TotalMilliseconds));
                _context.SaveChanges();
            }
        }
    }
}