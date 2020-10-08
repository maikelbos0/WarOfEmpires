namespace WarOfEmpires.Models.Alliances {
    public sealed class CurrentAllianceRightsViewModel {
        public bool IsInAlliance { get; set; }
        public bool CanInvite { get; set; }
        public bool CanManageRoles { get; set; }
        // TODO add can delete chat messages
    }
}