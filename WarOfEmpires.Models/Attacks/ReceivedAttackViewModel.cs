using System;

namespace WarOfEmpires.Models.Attacks {
    public sealed class ReceivedAttackViewModel : ViewModel {
        public DateTime Date { get; set; }
        public string DateString { get { return Date.ToString("yyyy-MM-dd HH:mm"); } }
        public int DefenderSoldierCasualties { get; set; }
        public int DefenderMercenaryCasualties { get; set; }
        public string Attacker { get; set; }
        public int AttackerSoldierCasualties { get; set; }
        public int AttackerMercenaryCasualties { get; set; }
        public int Turns { get; set; }
        public string Result { get; set; }
    }
}