using System.Collections.Generic;
using WarOfEmpires.Domain.Attacks;

namespace WarOfEmpires.Database.ReferenceEntities {
    public class TroopTypeEntity : BaseReferenceEntity<TroopType> {
        public virtual ICollection<AttackRound> AttackRounds { get; set; }
        public virtual ICollection<Troops> Troops { get; set; }
        public virtual ICollection<Casualties> Casualties { get; set; }
    }
}
