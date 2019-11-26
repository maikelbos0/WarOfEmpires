using System.Collections.Generic;
using WarOfEmpires.Domain.Empires;

namespace WarOfEmpires.Database.ReferenceEntities {
    internal class BuildingTypeEntity : BaseReferenceEntity<BuildingType> {
        public virtual ICollection<Building> Buildings { get; set; }
    }
}