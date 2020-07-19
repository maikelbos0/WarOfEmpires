using System;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.Models.Messages {
    [GridSorting(nameof(Date), true)]
    public sealed class ReceivedMessageViewModel : EntityViewModel {
        public DateTime Date { get; set; }
        [GridColumn(0, 25, "Received", SortData = nameof(Date), ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromMedium)]
        public string DateString { get { return Date.ToString(StringFormat.Date); } }
        [GridColumn(1, 25, "Sender")]
        public string Sender { get; set; }
        [GridColumn(2, 50, "Subject")]
        public string Subject { get; set; }
        public bool IsRead { get; set; }
    }
}