﻿using Microsoft.AspNetCore.Mvc;
using WarOfEmpires.Services;

namespace WarOfEmpires.Filters {
    public sealed class AllianceAuthorizeAttribute : TypeFilterAttribute, IAllianceAuthorizationRequest {
        public bool CanInvite { get; set; }
        public bool CanManageRoles { get; set; }
        public bool CanDeleteChatMessages { get; set; }
        public bool CanKickMembers { get; set; }
        public bool CanTransferLeadership { get; set; }
        public bool CanDisbandAlliance { get; set; }
        public bool CanManageNonAggressionPacts { get; set; }
        public bool CanManageWars { get; set; }
        public bool CanBank { get; set; }

        public AllianceAuthorizeAttribute() : base(typeof(AllianceAuthorizeFilter)) {
            Arguments = new object[] { this };
        }
    }
}
