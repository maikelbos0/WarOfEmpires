﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Empires {
    public sealed class TroopModel {
        [DisplayName ("Archers")]
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Archers must be a valid number")]
        public string Archers { get; set; }
        [DisplayName ("Archer Mercenaries")]
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Archer mercenaries must be a valid number")]
        public string MercenaryArchers { get; set; }
        [DisplayName ("Cavalry")]
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Cavalry must be a valid number")]
        public string Cavalry { get; set; }
        [DisplayName ("Cavalry Mercenaries")]
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Cavalry mercenaries must be a valid number")]
        public string MercenaryCavalry { get; set; }
        [DisplayName ("Footmen")]
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Footmen must be a valid number")]
        public string Footmen { get; set; }
        [DisplayName ("Footman Mercenaries")]
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Footman mercenaries must be a valid number")]
        public string MercenaryFootmen { get; set; }
        public int CurrentPeasants { get; set; }
        public int CurrentArchers { get; set; }
        public int CurrentMercenaryArchers { get; set; }
        public int CurrentCavalry { get; set; }
        public int CurrentMercenaryCavalry { get; set; }
        public int CurrentFootmen { get; set; }
        public int CurrentMercenaryFootmen { get; set; }
        public string Command { get; set; }
    }
}
