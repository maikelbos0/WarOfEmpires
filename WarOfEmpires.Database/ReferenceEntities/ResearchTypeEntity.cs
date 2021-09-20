using System.Collections.Generic;
using WarOfEmpires.Domain.Empires;

namespace WarOfEmpires.Database.ReferenceEntities {
    public class ResearchTypeEntity : BaseReferenceEntity<ResearchType> {
        public virtual ICollection<Research> Research { get; set; }
        public virtual ICollection<QueuedResearch> QueuedResearch { get; set; }
    }
}
