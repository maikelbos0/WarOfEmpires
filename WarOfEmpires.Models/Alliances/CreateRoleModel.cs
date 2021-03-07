using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Alliances {
    public sealed class CreateRoleModel {
        [DisplayName("Name")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [DisplayName("Can invite new members")]
        public bool CanInvite { get; set; }
        [DisplayName("Can manage alliance roles")]
        public bool CanManageRoles { get; set; }
        [DisplayName("Can delete messages from chat")]
        public bool CanDeleteChatMessages { get; set; }
        [DisplayName("Can kick members")]
        public bool CanKickMembers { get; set; }
        [DisplayName("Can manage non-aggression pacts")]
        public bool CanManageNonAggressionPacts { get; set; }
    }
}