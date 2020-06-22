using System;

namespace WarOfEmpires.Models.Alliances {
    public sealed class InviteViewModel : EntityViewModel {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public bool IsRead { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; }
    }
}