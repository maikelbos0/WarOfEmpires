using System.Collections.Generic;

namespace WarOfEmpires.Models.Empires {
    public sealed class BuildingUpgradesViewModel {
        public string Name { get; set; }
        public List<BuildingUpgradeViewModel> Upgrades { get; set; }
    }
}