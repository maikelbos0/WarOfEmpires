using System.Collections.Generic;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Events;

namespace WarOfEmpires.Repositories.Events {
    [ScopedServiceImplementation(typeof(IScheduledTaskRepository))]
    public class ScheduledTaskRepository : BaseRepository, IScheduledTaskRepository {
        public ScheduledTaskRepository(ILazyWarContext context) : base(context) { }

        public IEnumerable<ScheduledTask> GetAll() {
            return _context.ScheduledTasks.ToList();
        }
    }
}