using System.Collections.Generic;
using WarOfEmpires.Domain.Empires;

namespace WarOfEmpires.Database.ReferenceEntities {
    public class WorkerTypeEntity : BaseReferenceEntity<WorkerType> {
        public virtual ICollection<Workers> Workers { get; set; }
    }
}