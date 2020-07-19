using System;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.Models.Alliances {
    [GridSorting(nameof(Date), true)]
    public sealed class ReceivedInviteViewModel : EntityViewModel {
        public DateTime Date { get; set; }
        [GridColumn(0, 25, "Date", SortData = nameof(Date), ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromMedium)]
        public string DateString { get { return Date.ToString(StringFormat.Date); } }
        public int AllianceId { get; set; }
        [GridColumn(1, 10, "Alliance code", ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromMedium)]
        public string AllianceCode { get; set; }
        [GridColumn(2, 25, "Alliance name")]
        public string AllianceName { get; set; }
        [GridColumn(3, 40, "Subject")]
        public string Subject { get; set; }
        public bool IsRead { get; set; }
    }
}