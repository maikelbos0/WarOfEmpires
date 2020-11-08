using WarOfEmpires.Models.Grids;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.Models.Alliances {    
    public sealed class RoleViewModel : EntityViewModel {
        [GridColumn(0, 25, "Name")]
        public string Name { get; set; }
        public bool CanInvite { get; set; }
        [GridColumn(1, 15, "Can invite", ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromSmall)]
        public string CanInviteString { get { return CanInvite ? "Yes" : "No"; } }
        public bool CanManageRoles { get; set; }
        [GridColumn(2, 15, "Can manage roles", ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromSmall)]
        public string CanManageRolesString { get { return CanManageRoles ? "Yes" : "No"; } }
        public bool CanDeleteChatMessages { get; set; }
        [GridColumn(3, 15, "Can delete chat", ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromSmall)]
        public string CanDeleteChatMessagesString { get { return CanDeleteChatMessages ? "Yes" : "No"; } }       
        public bool CanKickMembers { get; set; }
        [GridColumn(4, 15, "Can kick members", ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromSmall)]
        public string CanKickMembersString { get { return CanKickMembers ? "Yes" : "No"; } }
        public int Players { get; set; }
        [GridColumn(5, 15, "Players", SortData = nameof(Players), ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromMedium)]
        public string PlayersString { get { return Players.ToString(StringFormat.Integer); } }
    }
}