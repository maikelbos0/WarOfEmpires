using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Security {
    public sealed class LogInUserModel {
        [DisplayName("Email address")]
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "This email address is invalid")]
        public string Email { get; set; }

        [DisplayName("Password")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
