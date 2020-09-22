using System;

namespace WarOfEmpires.Models.Alliances {
    public sealed class ReceivedInviteDetailsViewModel : EntityViewModel {
        public DateTime Date { get; set; }
        public int AllianceId { get; set; }
        public string AllianceCode { get; set; }
        public string AllianceName { get; set; }
        public bool IsRead { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Command { get; set; }
    }
}