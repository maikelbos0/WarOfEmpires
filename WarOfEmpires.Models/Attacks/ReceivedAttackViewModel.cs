using System;
using WarOfEmpires.Models.Formatting;
using WarOfEmpires.Models.Grids;

namespace WarOfEmpires.Models.Attacks {
    [GridSorting(nameof(Date), true)]
    public sealed class ReceivedAttackViewModel : EntityViewModel {        
        public DateTime Date { get; set; }
        [GridColumn(0, 15, "Date", SortData = nameof(Date), ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromSmall)]
        public string DateString { get { return Date.ToString(StringFormat.Date); } }
        [GridColumn(1, 8, "Type")]
        public string Type { get; set; }
        public int Turns { get; set; }
        [GridColumn(2, 6, "Turns", SortData = nameof(Turns), ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromLarge)]
        public string TurnsString { get { return Turns.ToString(StringFormat.Integer); } }
        public int DefenderSoldierCasualties { get; set; }
        [GridColumn(3, 10, "Soldiers lost", SortData = nameof(DefenderSoldierCasualties))]
        public string DefenderSoldierCasualtiesString { get { return DefenderSoldierCasualties.ToString(StringFormat.Integer); } }
        public int DefenderMercenaryCasualties { get; set; }
        [GridColumn(4, 10, "Mercs lost", SortData = nameof(DefenderMercenaryCasualties))]
        public string DefenderMercenaryCasualtiesString { get { return DefenderMercenaryCasualties.ToString(StringFormat.Integer); } }
        [GridColumn(5, 15, "Attacker")]
        public string Attacker { get; set; }
        [GridColumn(6, 7, "Alliance", ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromMedium)]
        public string AttackerAlliance { get; set; }
        public int AttackerSoldierCasualties { get; set; }
        [GridColumn(7, 10, "Soldiers lost", SortData = nameof(AttackerSoldierCasualties), ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromLarge)]
        public string AttackerSoldierCasualtiesString { get { return AttackerSoldierCasualties.ToString(StringFormat.Integer); } }
        public int AttackerMercenaryCasualties { get; set; }
        [GridColumn(8, 10, "Mercs lost", SortData = nameof(AttackerMercenaryCasualties), ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromLarge)]
        public string AttackerMercenaryCasualtiesString { get { return AttackerMercenaryCasualties.ToString(StringFormat.Integer); } }
        [GridColumn(9, 9, "Result")]
        public string Result { get; set; }
        public bool IsRead { get; set; }
    }
}