namespace WarOfEmpires.Models.Empires {
    public sealed class SiegeModel {
        public SiegeWeaponModel FireArrows { get; set; }
        public SiegeWeaponModel BatteringRams { get; set; }
        public SiegeWeaponModel ScalingLadders { get; set; }
        public int Engineers { get; set; }
        public int TotalMaintenance { get; set; }
        public int AvailableMaintenance { get; set; }
        public string Command { get; set; }
    }
}