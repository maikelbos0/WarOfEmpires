using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Alliances {
    public sealed class CreateRoleModel {
        [DisplayName("Name")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}