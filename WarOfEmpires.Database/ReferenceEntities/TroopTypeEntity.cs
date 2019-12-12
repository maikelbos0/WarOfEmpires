using System.Collections.Generic;
using WarOfEmpires.Domain.Attacks;

namespace WarOfEmpires.Database.ReferenceEntities {
    internal class TroopTypeEntity : BaseReferenceEntity<TroopType> {
        public virtual ICollection<AttackRound> AttackRounds { get; set; }
    }
}