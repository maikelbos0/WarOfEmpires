using System;

namespace WarOfEmpires.Models.Messages {
    public sealed class SentMessageViewModel {
        public string Recipient { get; set; }
        public DateTime Date { get; set; }
        public string DateString { get { return Date.ToString("yyyy-MM-dd HH:mm"); } }
        public string Subject { get; set; }
        public bool IsRead { get; set; }
    }
}