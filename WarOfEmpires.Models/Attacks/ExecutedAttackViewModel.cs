using System;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.Models.Attacks {
    public sealed class ExecutedAttackViewModel : EntityViewModel {
        public DateTime Date { get; set; }
        public string DateString { get { return Date.ToString(StringFormat.Date); } }
        public int AttackerSoldierCasualties { get; set; }
        public string AttackerSoldierCasualtiesString { get { return AttackerSoldierCasualties.ToString(StringFormat.Integer); } }
        public int AttackerMercenaryCasualties { get; set; }
        public string AttackerMercenaryCasualtiesString { get { return AttackerMercenaryCasualties.ToString(StringFormat.Integer); } }
        public string Defender { get; set; }
        public int DefenderSoldierCasualties { get; set; }
        public string DefenderSoldierCasualtiesString { get { return DefenderSoldierCasualties.ToString(StringFormat.Integer); } }
        public int DefenderMercenaryCasualties { get; set; }
        public string DefenderMercenaryCasualtiesString { get { return DefenderMercenaryCasualties.ToString(StringFormat.Integer); } }
        public int Turns { get; set; }
        public string TurnsString { get { return Turns.ToString(StringFormat.Integer); } }
        public string Result { get; set; }
    }
}