using System.Collections.Generic;
using WarOfEmpires.Domain.Siege;

namespace WarOfEmpires.Database.ReferenceEntities {
    internal class SiegeWeaponTypeEntity : BaseReferenceEntity<SiegeWeaponType> {
        public virtual ICollection<SiegeWeapon> SiegeWeapons { get; set; }
    }
}
