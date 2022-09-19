namespace WarOfEmpires.Domain.Alliances {
    [System.Flags]
    public enum Right {
        None = 0,
        CanInvite = 1,
        CanManageRoles = 2,
        CanDeleteChatMessages = 4,
        CanKickMembers = 8,
        CanManageNonAggressionPacts = 16,
        CanManageWars = 32,
        CanBank = 64
    }
}
