using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Alliances {
    public sealed class CreateAllianceModel {
        [DisplayName("Code")]
        [Required(ErrorMessage = "Code is required")]
        [MaxLength(4, ErrorMessage = "Code must be 4 characters or less")]
        public string Code { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}