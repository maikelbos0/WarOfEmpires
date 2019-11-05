using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Security {
    public sealed class ChangeUserEmailModel {
        [DisplayName("Password")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [DisplayName("New email address")]
        [Required(ErrorMessage = "New email address is required")]
        public string NewEmail { get; set; }

        [DisplayName("Confirm new email address")]
        [Required(ErrorMessage = "Confirm new email address is required")]
        [Compare("NewEmail", ErrorMessage = "New email address and confirm new email address must match")]
        public string ConfirmNewEmail { get; set; }
    }
}