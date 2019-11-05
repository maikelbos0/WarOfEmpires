using WarOfEmpires.Queries;
using WarOfEmpires.Domain.Auditing;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Serialization;
using WarOfEmpires.Database;

namespace WarOfEmpires.QueryHandlers.Decorators {
    public sealed class AuditDecorator<TQuery, TReturnValue> : Decorator<IQueryHandler<TQuery, TReturnValue>>, IQueryHandler<TQuery, TReturnValue> where TQuery : IQuery<TReturnValue> {
        private readonly IWarContext _context;
        private readonly ISerializer _serializer;

        public AuditDecorator(IWarContext context, ISerializer serializer) {
            _context= context;
            _serializer = serializer;
        }

        public TReturnValue Execute(TQuery query) {
            // Explicitly use the context to prevent having to use a repository
            _context.QueryExecutions.Add(new QueryExecution(typeof(TQuery).FullName, _serializer.SerializeToJson(query)));
            _context.SaveChanges();

            return Handler.Execute(query);
        }
    }
}