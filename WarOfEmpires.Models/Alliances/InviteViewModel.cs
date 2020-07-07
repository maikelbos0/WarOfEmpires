using System;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.Models.Alliances {
    [GridSorting(nameof(Date), true)]
    public sealed class InviteViewModel : EntityViewModel {
        public DateTime Date { get; set; }
        [GridColumn(0, 15, "Date", SortData = nameof(Date))]
        public string DateString { get { return Date.ToString(StringFormat.Date); } }
        public int PlayerId { get; set; }
        [GridColumn(1, 30, "Recipient")]
        public string PlayerName { get; set; }
        public bool IsRead { get; set; }
        [GridColumn(3, 55, "Subject")]
        public string Subject { get; set; }
    }
}