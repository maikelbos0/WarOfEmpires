using System.Collections.Generic;

namespace WarOfEmpires.Models.Alliances {
    public sealed class InvitesViewModel {
        public string Name { get; set; }
        public List<InviteViewModel> Invites { get; set; }
    }
}