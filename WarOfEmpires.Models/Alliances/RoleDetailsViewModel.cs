using System.Collections.Generic;

namespace WarOfEmpires.Models.Alliances {
    public sealed class RoleDetailsViewModel : EntityViewModel {
        public string Name { get; set; }
        public List<RolePlayerViewModel> Players { get; set; }
    }
}