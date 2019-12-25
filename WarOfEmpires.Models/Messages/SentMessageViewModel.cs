using System;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.Models.Messages {
    public sealed class SentMessageViewModel : EntityViewModel {
        public string Recipient { get; set; }
        public DateTime Date { get; set; }
        public string DateString { get { return Date.ToString(StringFormat.Date); } }
        public string Subject { get; set; }
        public bool IsRead { get; set; }
    }
}