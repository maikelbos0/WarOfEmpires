using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Alliances {
    public sealed class SendInviteModel {
        public string PlayerId { get; set; }
        public string PlayerName { get; set; }
        [DisplayName("Subject")]
        [Required(ErrorMessage = "Subject is required")]
        [MaxLength(100, ErrorMessage = "Subject can only be 100 characters long")]
        public string Subject { get; set; }
        [DisplayName("Body")]
        public string Body { get; set; }
    }
}