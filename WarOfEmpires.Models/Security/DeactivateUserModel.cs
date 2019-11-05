using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Security {
    public sealed class DeactivateUserModel {

        [DisplayName("Password")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}