using System.Net;
using System.Web.Mvc;
using Unity;
using WarOfEmpires.Services;

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
            if (!UnityConfig.Container.Resolve<IAuthorizationService>().IsAuthorized(this)) {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
        }
    }
}