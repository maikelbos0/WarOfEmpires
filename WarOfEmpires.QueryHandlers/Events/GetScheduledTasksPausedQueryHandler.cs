using WarOfEmpires.Queries.Events;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Events {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetScheduledTasksPausedQueryHandler : IQueryHandler<GetScheduledTasksPausedQuery, bool?> {
        public bool? Execute(GetScheduledTasksPausedQuery query) {
            throw new System.NotImplementedException();
        }
    }
}