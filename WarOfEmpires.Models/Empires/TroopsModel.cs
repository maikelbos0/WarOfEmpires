using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Empires {
    public sealed class TroopsModel {
        [DisplayName("Stamina")]
        [RegularExpression("^\\d{1,3}$", ErrorMessage = "Stamina to heal must be a valid number")]
        public string StaminaToHeal { get; set; }
        public int CurrentPeasants { get; set; }
        public List<TroopModel> Troops { get; set; }
        public ResourcesViewModel MercenaryTrainingCost { get; set; }
        public bool WillUpkeepRunOut { get; set; }
        public bool HasUpkeepRunOut { get; set; }
        public bool HasSoldierShortage { get; set; }
        public string Command { get; set; }
        public int CurrentStamina { get; set; }
    }
}