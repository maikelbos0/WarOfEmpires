using System.Collections.Generic;
using WarOfEmpires.Domain.Attacks;

namespace WarOfEmpires.Database.ReferenceEntities {
    internal sealed class AttackResultEntity : BaseReferenceEntity<AttackResult> {
        public ICollection<Attack> Attacks { get; set; }
    }
}