namespace WarOfEmpires.Models.Security {
    public sealed class UserClaimsViewModel {
        public string Subject { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsPlayer { get; set; }
        public bool IsInAlliance { get; set; }
        public bool CanInvite { get; set; }
        public bool CanManageRoles { get; set; }
        public bool CanLeaveAlliance { get; set; }
        public bool CanTransferLeadership { get; set; }
        public bool CanDisbandAlliance { get; set; }
        public bool CanManageNonAggressionPacts { get; set; }
        public bool CanManageWars { get; set; }
        public bool CanBank { get; set; }
        public string DisplayName { get; set; }
    }
}
