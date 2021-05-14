using System.Collections.Generic;
using WarOfEmpires.Domain.Events;

namespace WarOfEmpires.Database.ReferenceEntities {
    public class TaskExecutionModeEntity : BaseReferenceEntity<TaskExecutionMode> {
        public virtual ICollection<ScheduledTask> ScheduledTasks { get; set; }
    }
}