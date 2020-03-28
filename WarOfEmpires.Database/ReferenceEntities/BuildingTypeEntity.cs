using System.Collections.Generic;
using WarOfEmpires.Domain.Empires;

namespace WarOfEmpires.Database.ReferenceEntities {
    internal sealed class BuildingTypeEntity : BaseReferenceEntity<BuildingType> {
        public ICollection<Building> Buildings { get; set; }
    }
}