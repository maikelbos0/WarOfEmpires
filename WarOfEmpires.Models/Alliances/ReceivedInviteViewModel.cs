using System;

namespace WarOfEmpires.Models.Alliances {
    public sealed class ReceivedInviteViewModel : EntityViewModel {
        public int AllianceId { get; set; }
        public string AllianceCode { get; set; }
        public string AllianceName { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
    }
}