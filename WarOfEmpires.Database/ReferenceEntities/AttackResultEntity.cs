using System.Collections.Generic;
using WarOfEmpires.Domain.Attacks;

namespace WarOfEmpires.Database.ReferenceEntities {
    public class AttackResultEntity : BaseReferenceEntity<AttackResult> {
        public virtual ICollection<Attack> Attacks { get; set; }
    }
}
