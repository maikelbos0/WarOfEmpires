using System;
using System.Collections.Generic;
using WarOfEmpires.Models.Empires;

namespace WarOfEmpires.Models.Attacks {
    public sealed class AttackDetailsViewModel : ViewModel {
        public DateTime Date { get; set; }
        public string DateString { get { return Date.ToString("yyyy-MM-dd HH:mm"); } }
        public string Attacker { get; set; }
        public string Defender { get; set; }
        public int Turns { get; set; }
        public List<AttackRoundDetailsViewModel> Rounds { get; set; } = new List<AttackRoundDetailsViewModel>();
        public string Result { get; set; }
        public ResourcesViewModel Resources { get; set; }
    }
}