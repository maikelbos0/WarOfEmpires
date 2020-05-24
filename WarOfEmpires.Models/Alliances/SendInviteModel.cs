using System.ComponentModel;

namespace WarOfEmpires.Models.Alliances {
    public sealed class SendInviteModel {
        public string PlayerId { get; set; }
        public string PlayerName { get; set; }
        [DisplayName("Message")]
        public string Message { get; set; }
    }
}