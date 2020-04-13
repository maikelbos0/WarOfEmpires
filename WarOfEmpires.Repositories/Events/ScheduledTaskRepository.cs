using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Events;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.Repositories.Events {
    [InterfaceInjectable]
    public class ScheduledTaskRepository : IScheduledTaskRepository {
        private readonly IWarContext _context;

        public ScheduledTaskRepository(IWarContext context) {
            _context = context;
        }

        public IEnumerable<ScheduledTask> GetAll() {
            return _context.ScheduledTasks.ToList();
        }

        public void Update() {
            _context.SaveChanges();
        }
    }
}