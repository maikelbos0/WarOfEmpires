using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Empires {
    public sealed class TroopModel {
        [DisplayName("Soldiers")]
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Invalid number")]
        public int? Soldiers { get; set; }
        [DisplayName("Mercenaries")]
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Invalid number")]
        public int? Mercenaries { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public ResourcesViewModel Cost { get; set; }
        public int CurrentSoldiers { get; set; }
        public int CurrentMercenaries { get; set; }
    }
}