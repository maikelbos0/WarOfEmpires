using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Messages {
    public sealed class MessageModel {
        public int RecipientId { get; set; }
        public string Recipient { get; set; }

        [DisplayName("Subject")]
        [Required(ErrorMessage = "Subject is required")]
        [MaxLength(100, ErrorMessage = "Subject can only be 100 characters long")]
        public string Subject { get; set; }

        [DisplayName("Body")]
        public string Body { get; set; }
    }
}