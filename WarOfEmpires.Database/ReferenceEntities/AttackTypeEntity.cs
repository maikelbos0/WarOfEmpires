using System.Collections.Generic;
using WarOfEmpires.Domain.Attacks;

namespace WarOfEmpires.Database.ReferenceEntities {
    public class AttackTypeEntity : BaseReferenceEntity<AttackType> {
        public virtual ICollection<Attack> Attacks { get; set; }
    }
}