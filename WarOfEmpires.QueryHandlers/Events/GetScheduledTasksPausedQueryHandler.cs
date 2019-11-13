using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Queries.Events;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Events {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetScheduledTasksPausedQueryHandler : IQueryHandler<GetScheduledTasksPausedQuery, bool?> {
        private readonly IWarContext _context;

        public GetScheduledTasksPausedQueryHandler(IWarContext context) {
            _context = context;
        }

        public bool? Execute(GetScheduledTasksPausedQuery query) {
            var taskPausedStatus = _context.ScheduledTasks.Select(t => t.IsPaused).Distinct().ToList();

            if (taskPausedStatus.Count == 1) {
                return taskPausedStatus.Single();
            }
            else {
                return null;
            }
        }
    }
}