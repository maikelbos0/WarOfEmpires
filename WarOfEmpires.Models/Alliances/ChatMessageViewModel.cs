using System;

namespace WarOfEmpires.Models.Alliances {
    public sealed class ChatMessageViewModel : EntityViewModel {
        public int? PlayerId { get; set; }
        public string Player { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
    }
}