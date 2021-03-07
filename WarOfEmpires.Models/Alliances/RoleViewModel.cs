using WarOfEmpires.Models.Grids;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.Models.Alliances {    
    public sealed class RoleViewModel : EntityViewModel {
        [GridColumn(0, 20, "Name")]
        public string Name { get; set; }
        public bool CanInvite { get; set; }
        [GridColumn(1, 14, "Can invite", ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromSmall)]
        public string CanInviteString { get { return CanInvite ? "Yes" : "No"; } }
        public bool CanManageRoles { get; set; }
        [GridColumn(2, 14, "Can manage roles", ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromSmall)]
        public string CanManageRolesString { get { return CanManageRoles ? "Yes" : "No"; } }
        public bool CanDeleteChatMessages { get; set; }
        [GridColumn(3, 14, "Can delete chat", ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromSmall)]
        public string CanDeleteChatMessagesString { get { return CanDeleteChatMessages ? "Yes" : "No"; } }       
        public bool CanKickMembers { get; set; }
        [GridColumn(4, 14, "Can kick members", ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromSmall)]
        public string CanKickMembersString { get { return CanKickMembers ? "Yes" : "No"; } }
        public bool CanManageNonAggressionPacts { get; set; }
        [GridColumn(5, 14, "Can manage pacts", ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromSmall)]
        public string CanManageNonAggressionPactsString { get { return CanManageNonAggressionPacts ? "Yes" : "No"; } }
        public int Players { get; set; }
        [GridColumn(5, 10, "Players", SortData = nameof(Players), ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromMedium)]
        public string PlayersString { get { return Players.ToString(StringFormat.Integer); } }
    }
}