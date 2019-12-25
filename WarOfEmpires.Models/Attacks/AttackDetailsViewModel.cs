using System;
using System.Collections.Generic;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.Models.Attacks {
    public sealed class AttackDetailsViewModel : EntityViewModel {
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
        public string Attacker { get; set; }
        public string Defender { get; set; }
        public int Turns { get; set; }
        public List<AttackRoundDetailsViewModel> Rounds { get; set; } = new List<AttackRoundDetailsViewModel>();
        public string Result { get; set; }
        public ResourcesViewModel Resources { get; set; }
    }
}