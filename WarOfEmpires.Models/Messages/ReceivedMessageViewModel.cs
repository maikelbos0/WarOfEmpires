using System;

namespace WarOfEmpires.Models.Messages {
    public sealed class ReceivedMessageViewModel : EntityViewModel {
        public string Sender { get; set; }
        public DateTime Date { get; set; }
        public string DateString { get { return Date.ToString("yyyy-MM-dd HH:mm"); } }
        public string Subject { get; set; }
        public bool IsRead { get; set; }
    }
}