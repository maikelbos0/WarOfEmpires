using System.Collections.Generic;

namespace WarOfEmpires.Models.Alliances {
    public sealed class RoleDetailsViewModel : EntityViewModel {
        public string Name { get; set; }
        public bool CanInvite { get; set; }
        public bool CanManageRoles { get; set; }
        public bool HasRights {
            get {
                return CanInvite || CanManageRoles;
            }
        }
        public List<RolePlayerViewModel> Players { get; set; }
    }
}