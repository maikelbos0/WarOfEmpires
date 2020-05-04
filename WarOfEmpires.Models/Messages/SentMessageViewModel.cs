using System;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.Models.Messages {
    [GridSorting(nameof(Date), true)]
    public sealed class SentMessageViewModel : EntityViewModel {        
        public DateTime Date { get; set; }
        [GridColumn(0, 15, "Sent", SortData = nameof(Date))]
        public string DateString { get { return Date.ToString(StringFormat.Date); } }
        [GridColumn(1, 20, "Recipient")]
        public string Recipient { get; set; }
        [GridColumn(2, 65, "Subject")]
        public string Subject { get; set; }
        public bool IsRead { get; set; }
    }
}