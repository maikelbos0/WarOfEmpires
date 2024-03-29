﻿namespace WarOfEmpires.Services {
    public interface IAllianceAuthorizationRequest {
        bool CanInvite { get; set; }
        bool CanManageRoles { get; set; }
        bool CanDeleteChatMessages { get; set; }
        bool CanKickMembers { get; set; }
        bool CanTransferLeadership { get; set; }
        bool CanDisbandAlliance { get; set; }
        bool CanManageNonAggressionPacts { get; set; }
        bool CanManageWars { get; set; }
        bool CanBank { get; set; }
    }
}