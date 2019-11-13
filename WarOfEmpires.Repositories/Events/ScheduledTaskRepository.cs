using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Events;

namespace WarOfEmpires.Repositories.Events {
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