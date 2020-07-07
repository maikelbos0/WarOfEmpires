using System;

namespace WarOfEmpires.Models.Alliances {
    public sealed class InviteDetailsViewModel : EntityViewModel {
        public DateTime Date { get; set; }
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public bool IsRead { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}