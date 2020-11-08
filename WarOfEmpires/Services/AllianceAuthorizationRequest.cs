﻿namespace WarOfEmpires.Services {
    public sealed class AllianceAuthorizationRequest : IAllianceAuthorizationRequest {
        public bool CanInvite { get; set; }
        public bool CanManageRoles { get; set; }
        public bool CanDeleteChatMessages { get; set; }
        public bool CanKickMembers { get; set; }
    }
}