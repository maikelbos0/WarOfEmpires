using System;
using WarOfEmpires.Models.Formatting;
using WarOfEmpires.Models.Grids;

namespace WarOfEmpires.Models.Messages {
    [GridSorting(nameof(Date), true)]
    public sealed class SentMessageViewModel : EntityViewModel {        
        public DateTime Date { get; set; }
        [GridColumn(0, 25, "Sent", SortData = nameof(Date), ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromMedium)]
        public string DateString { get { return Date.ToString(StringFormat.Date); } }
        [GridColumn(1, 25, "Recipient")]
        public string Recipient { get; set; }
        [GridColumn(2, 50, "Subject")]
        public string Subject { get; set; }
        public bool IsRead { get; set; }
    }
}