using System.Collections.Generic;
using WarOfEmpires.Domain.Empires;

namespace WarOfEmpires.Database.ReferenceEntities {
    internal sealed class WorkerTypeEntity : BaseReferenceEntity<WorkerType> {
        public ICollection<Workers> Workers { get; set; }
    }
}