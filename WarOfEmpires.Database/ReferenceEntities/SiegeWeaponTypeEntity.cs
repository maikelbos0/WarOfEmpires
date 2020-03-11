using System.Collections.Generic;
using WarOfEmpires.Domain.Siege;

namespace WarOfEmpires.Database.ReferenceEntities {
    internal sealed class SiegeWeaponTypeEntity : BaseReferenceEntity<SiegeWeaponType> {
        public ICollection<SiegeWeapon> SiegeWeapons { get; set; }
    }
}