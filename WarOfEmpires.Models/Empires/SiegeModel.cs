using System.Collections.Generic;

namespace WarOfEmpires.Models.Empires {
    public sealed class SiegeModel {
        public List<SiegeWeaponModel> SiegeWeapons { get; set; }
        public int Engineers { get; set; }
        public int TotalMaintenance { get; set; }
        public int AvailableMaintenance { get; set; }
        public string Command { get; set; }
    }
}