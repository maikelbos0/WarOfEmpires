using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Empires {
    public sealed class SiegeModel {
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Fire arrows must be a valid number")]
        public string FireArrows { get; set; }
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Battering rams must be a valid number")]
        public string BatteringRams { get; set; }
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Scaling ladders must be a valid number")]
        public string ScalingLadders { get; set; }
        public SiegeWeaponInfoModel FireArrowsInfo { get; set; }
        public SiegeWeaponInfoModel BatteringRamsInfo { get; set; }
        public SiegeWeaponInfoModel ScalingLaddersInfo { get; set; }
        public int Engineers { get; set; }
        public int TotalMaintenance { get; set; }
        public int AvailableMaintenance { get; set; }
        public string Command { get; set; }
    }
}