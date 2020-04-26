using System.Collections.Generic;
using WarOfEmpires.Domain.Events;

namespace WarOfEmpires.Database.ReferenceEntities {
    internal sealed class TaskExecutionModeEntity : BaseReferenceEntity<TaskExecutionMode> {
        public ICollection<ScheduledTask> ScheduledTasks { get; set; }
    }
}