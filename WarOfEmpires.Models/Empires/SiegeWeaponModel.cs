using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Empires {
    public class SiegeWeaponModel {
        [DisplayName("Count")]
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Invalid number")]
        public int? Count { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public ResourcesViewModel Cost { get; set; }
        public int TroopCount { get; set; }
        public int Maintenance { get; set; }
        public string Description { get; set; }
        public int CurrentCount { get; set; }
        public int CurrentTroopCount { get; set; }
    }
}