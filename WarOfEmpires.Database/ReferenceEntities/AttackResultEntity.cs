using System.Collections.Generic;
using WarOfEmpires.Domain.Attacks;

namespace WarOfEmpires.Database.ReferenceEntities {
    internal class AttackResultEntity : BaseReferenceEntity<AttackResult> {
        public virtual ICollection<Attack> Attacks { get; set; }
    }
}