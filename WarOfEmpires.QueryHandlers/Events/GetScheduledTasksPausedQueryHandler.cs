﻿using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Queries.Events;

namespace WarOfEmpires.QueryHandlers.Events {
    public sealed class GetScheduledTasksPausedQueryHandler : IQueryHandler<GetScheduledTasksPausedQuery, bool?> {
        private readonly IReadOnlyWarContext _context;

        public GetScheduledTasksPausedQueryHandler(IReadOnlyWarContext context) {
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