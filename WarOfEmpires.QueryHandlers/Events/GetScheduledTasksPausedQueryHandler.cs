using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Queries.Events;
using WarOfEmpires.QueryHandlers.Decorators;

namespace WarOfEmpires.QueryHandlers.Events {
    [TransientServiceImplementation(typeof(IQueryHandler<GetScheduledTasksPausedQuery, bool?>))]
    public sealed class GetScheduledTasksPausedQueryHandler : IQueryHandler<GetScheduledTasksPausedQuery, bool?> {
        private readonly IWarContext _context;

        public GetScheduledTasksPausedQueryHandler(IWarContext context) {
            _context = context;
        }

        [Audit]
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