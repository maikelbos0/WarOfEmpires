using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Security {
    public sealed class ChangeUserPasswordModel {
        [DisplayName("Current password")]
        [Required(ErrorMessage = "Current password is required")]
        public string CurrentPassword { get; set; }

        [DisplayName("New password")]
        [Required(ErrorMessage = "New password is required")]
        public string NewPassword { get; set; }

        [DisplayName("Confirm new password")]
        [Required(ErrorMessage = "Confirm new password is required")]
        [Compare("NewPassword", ErrorMessage = "New password and confirm new password must match")]
        public string ConfirmNewPassword { get; set; }
    }
}