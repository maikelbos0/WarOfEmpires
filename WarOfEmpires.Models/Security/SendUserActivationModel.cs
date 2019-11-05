using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Security {
    public sealed class SendUserActivationModel {
        [DisplayName("Email address")]
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "This email address is invalid")]
        public string Email { get; set; }
    }
}