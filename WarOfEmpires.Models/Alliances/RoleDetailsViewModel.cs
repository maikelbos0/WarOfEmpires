using System.Collections.Generic;

namespace WarOfEmpires.Models.Alliances {
    public sealed class RoleDetailsViewModel : EntityViewModel {
        public string Name { get; set; }
        public bool CanInvite { get; set; }
        public bool CanManageRoles { get; set; }
        public bool CanDeleteChatMessages { get; set; }
        public bool HasRights {
            get {
                return CanInvite || CanManageRoles || CanDeleteChatMessages;
            }
        }
        public List<RolePlayerViewModel> Players { get; set; }
    }
}