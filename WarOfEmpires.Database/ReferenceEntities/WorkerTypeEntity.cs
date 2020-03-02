using System.Collections.Generic;
using WarOfEmpires.Domain.Empires;

namespace WarOfEmpires.Database.ReferenceEntities {
    internal class WorkerTypeEntity : BaseReferenceEntity<WorkerType> {
        public virtual ICollection<Workers> Workers { get; set; }
    }
}