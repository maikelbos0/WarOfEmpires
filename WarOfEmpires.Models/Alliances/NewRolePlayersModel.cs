using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Alliances {
    public sealed class NewRolePlayersModel : EntityViewModel {        
        public string Name { get; set; }
        [DisplayName("Player")]
        [Required(ErrorMessage = "Player is required")]
        public int PlayerId { get; set; }

        public List<NewRolePlayerModel> Players { get; set; }
    }
}