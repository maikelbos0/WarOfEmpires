using System.Collections.Generic;
using WarOfEmpires.Domain.Attacks;

namespace WarOfEmpires.Database.ReferenceEntities {
    internal sealed class AttackTypeEntity : BaseReferenceEntity<AttackType> {
        public ICollection<Attack> Attacks { get; set; }
    }
}