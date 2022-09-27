using WarOfEmpires.Models.Formatting;
using WarOfEmpires.Models.Grids;

namespace WarOfEmpires.Models.Alliances {    
    public sealed class RoleViewModel : EntityViewModel {
        [GridColumn(0, 20, "Name")]
        public string Name { get; set; }
        public bool CanInvite { get; set; }
        [GridColumn(1, 10, "Invite", ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromSmall)]
        public string CanInviteString { get { return CanInvite ? "Yes" : "No"; } }
        public bool CanManageRoles { get; set; }
        [GridColumn(2, 10, "Manage roles", ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromSmall)]
        public string CanManageRolesString { get { return CanManageRoles ? "Yes" : "No"; } }
        public bool CanDeleteChatMessages { get; set; }
        [GridColumn(3, 10, "Delete chat", ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromSmall)]
        public string CanDeleteChatMessagesString { get { return CanDeleteChatMessages ? "Yes" : "No"; } }       
        public bool CanKickMembers { get; set; }
        [GridColumn(4, 10, "Kick members", ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromSmall)]
        public string CanKickMembersString { get { return CanKickMembers ? "Yes" : "No"; } }
        public bool CanManageNonAggressionPacts { get; set; }
        [GridColumn(5, 10, "Manage pacts", ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromSmall)]
        public string CanManageNonAggressionPactsString { get { return CanManageNonAggressionPacts ? "Yes" : "No"; } }
        public bool CanManageWars { get; set; }
        [GridColumn(6, 10, "Manage wars", ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromSmall)]
        public string CanManageWarsString { get { return CanManageWars ? "Yes" : "No"; } }
        public bool CanBank { get; set; }
        [GridColumn(7, 10, "Bank", ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromSmall)]
        public string CanBankString { get { return CanBank ? "Yes" : "No"; } }
        public int Players { get; set; }
        [GridColumn(8, 10, "Players", SortData = nameof(Players), ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromMedium)]
        public string PlayersString { get { return Players.ToString(StringFormat.Integer); } }
    }
}