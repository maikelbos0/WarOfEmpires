using System.Net;
using Unity;
using WarOfEmpires.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WarOfEmpires.Attributes {
    public sealed class AllianceAuthorizeAttribute : ActionFilterAttribute, IAllianceAuthorizationRequest {
        public bool CanInvite { get; set; }
        public bool CanManageRoles { get; set; }
        public bool CanDeleteChatMessages { get; set; }
        public bool CanKickMembers { get; set; }
        public bool CanTransferLeadership { get; set; }
        public bool CanDisbandAlliance { get; set; }
        public bool CanManageNonAggressionPacts { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            // TODO determine if this is still the way to do custom authorization
            if (!UnityConfig.Container.Resolve<IAuthorizationService>().IsAuthorized(this)) {
                filterContext.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
            }
        }
    }
}