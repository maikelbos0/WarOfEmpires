using System.Collections.Generic;
using WarOfEmpires.Domain.Events;

namespace WarOfEmpires.Repositories.Events {
    public interface IScheduledTaskRepository {
        IEnumerable<ScheduledTask> GetAll();
        void Update();
    }
}