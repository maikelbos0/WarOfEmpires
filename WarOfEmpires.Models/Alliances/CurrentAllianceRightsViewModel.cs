namespace WarOfEmpires.Models.Alliances {
    public sealed class CurrentAllianceRightsViewModel {
        public bool IsInAlliance { get; set; }
        public bool CanInvite { get; set; }
        public bool CanManageRoles { get; set; }
        public bool CanDeleteChatMessages { get; set; }
        public bool CanKickMembers { get; set; }
        public bool CanTransferLeadership { get; set; }
        public bool CanDisbandAlliance { get; set; }
        public bool CanManageNonAggressionPacts { get; set; }
        public bool CanManageWars { get; set; }
    }
}