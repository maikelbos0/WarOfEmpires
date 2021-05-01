using System.Collections.Generic;
using WarOfEmpires.Domain.Attacks;

namespace WarOfEmpires.Database.ReferenceEntities {
    internal sealed class TroopTypeEntity : BaseReferenceEntity<TroopType> {
        public ICollection<AttackRound> AttackRounds { get; set; }
        public ICollection<Troops> Troops { get; set; }
        public ICollection<Casualties> Casualties { get; set; }
    }
}