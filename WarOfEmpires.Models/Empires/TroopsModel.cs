using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Empires {
    public sealed class TroopsModel {
        [DisplayName("Archers")]
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Archers must be a valid number")]
        public string Archers { get; set; }
        [DisplayName("Mercenary archers")]
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Mercenary archers must be a valid number")]
        public string MercenaryArchers { get; set; }
        [DisplayName("Cavalry")]
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Cavalry must be a valid number")]
        public string Cavalry { get; set; }
        [DisplayName("Mercenary cavalry")]
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Mercenary cavalry must be a valid number")]
        public string MercenaryCavalry { get; set; }
        [DisplayName("Footmen")]
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Footmen must be a valid number")]
        public string Footmen { get; set; }
        [DisplayName("Mercenary footmen")]
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Mercenary footmen must be a valid number")]
        public string MercenaryFootmen { get; set; }
        [DisplayName("Stamina")]
        [RegularExpression("^\\d{1,3}$", ErrorMessage = "Stamina to heal must be a valid number")]
        public string StaminaToHeal { get; set; }
        public int CurrentPeasants { get; set; }
        public TroopModel ArcherInfo { get; set; }
        public TroopModel CavalryInfo { get; set; }
        public TroopModel FootmanInfo { get; set; }
        public ResourcesViewModel MercenaryTrainingCost { get; set; }
        public bool WillUpkeepRunOut { get; set; }
        public bool HasUpkeepRunOut { get; set; }
        public bool HasSoldierShortage { get; set; }
        public string Command { get; set; }
        public int CurrentStamina { get; set; }
    }
}