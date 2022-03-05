using WarOfEmpires.Domain.Auditing;
using WarOfEmpires.Utilities.Serialization;
using System.Diagnostics;
using VDT.Core.DependencyInjection.Decorators;

namespace WarOfEmpires.Database.Auditing {
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

            _context.ActionExecutions.Add(new ActionExecution(context.TargetType.FullName, context.Method.Name, _serializer.SerializeToJson(context.Arguments), _stopwatch.Elapsed.TotalMilliseconds));
            _context.SaveChanges();
        }
    }
}