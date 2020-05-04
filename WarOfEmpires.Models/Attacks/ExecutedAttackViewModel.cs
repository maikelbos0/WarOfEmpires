using System;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.Models.Attacks {
    [GridSorting(nameof(Date), true)]
    public sealed class ExecutedAttackViewModel : EntityViewModel {
        public DateTime Date { get; set; }
        [GridColumn(0, 15, "Date", SortData = nameof(Date))]
        public string DateString { get { return Date.ToString(StringFormat.Date); } }
        [GridColumn(1, 10, "Type")]
        public string Type { get; set; }
        public int Turns { get; set; }
        [GridColumn(2, 10, "Turns", SortData = nameof(Turns))]
        public string TurnsString { get { return Turns.ToString(StringFormat.Integer); } }
        public int AttackerSoldierCasualties { get; set; }
        [GridColumn(3, 10, "Soldiers lost", SortData = nameof(AttackerSoldierCasualties))]
        public string AttackerSoldierCasualtiesString { get { return AttackerSoldierCasualties.ToString(StringFormat.Integer); } }
        public int AttackerMercenaryCasualties { get; set; }
        [GridColumn(4, 10, "Mercenaries lost", SortData = nameof(AttackerMercenaryCasualties))]
        public string AttackerMercenaryCasualtiesString { get { return AttackerMercenaryCasualties.ToString(StringFormat.Integer); } }
        [GridColumn(5, 15, "Defender")]
        public string Defender { get; set; }
        public int DefenderSoldierCasualties { get; set; }
        [GridColumn(6, 10, "Soldiers lost", SortData = nameof(DefenderSoldierCasualties))]
        public string DefenderSoldierCasualtiesString { get { return DefenderSoldierCasualties.ToString(StringFormat.Integer); } }
        public int DefenderMercenaryCasualties { get; set; }
        [GridColumn(7, 10, "Mercenaries lost", SortData = nameof(DefenderMercenaryCasualties))]
        public string DefenderMercenaryCasualtiesString { get { return DefenderMercenaryCasualties.ToString(StringFormat.Integer); } }
        [GridColumn(8, 10, "Result")]
        public string Result { get; set; }
    }
}