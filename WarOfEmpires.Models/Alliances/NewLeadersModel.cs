using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Alliances {
    public sealed class NewLeadersModel {
        [DisplayName("Member")]
        [Required(ErrorMessage = "Member is required")]
        public int? MemberId { get; set; }

        public List<NewLeaderModel> Members { get; set; }
    }
}
