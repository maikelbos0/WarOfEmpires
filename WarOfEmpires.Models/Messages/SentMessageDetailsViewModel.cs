using System;

namespace WarOfEmpires.Models.Messages {
    public sealed class SentMessageDetailsViewModel : EntityViewModel {
        public int? RecipientId { get; set; }
        public string Recipient { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsRead { get; set; }
    }
}