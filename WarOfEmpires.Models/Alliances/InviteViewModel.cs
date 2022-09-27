using System;
using WarOfEmpires.Models.Formatting;
using WarOfEmpires.Models.Grids;

namespace WarOfEmpires.Models.Alliances {
    [GridSorting(nameof(Date), true)]
    public sealed class InviteViewModel : EntityViewModel {
        public DateTime Date { get; set; }
        [GridColumn(0, 25, "Date", SortData = nameof(Date), ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromMedium)]
        public string DateString { get { return Date.ToString(StringFormat.Date); } }
        public int PlayerId { get; set; }
        [GridColumn(1, 25, "Recipient")]
        public string PlayerName { get; set; }
        public bool IsRead { get; set; }
        [GridColumn(3, 50, "Subject")]
        public string Subject { get; set; }
    }
}