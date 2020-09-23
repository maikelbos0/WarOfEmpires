using System.Net;
using System.Web.Mvc;
using Unity;
using WarOfEmpires.Services;

namespace WarOfEmpires.Attributes {
    public sealed class AllianceAuthorizeAttribute : ActionFilterAttribute {
        public bool CanInvite { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            var allianceRights = UnityConfig.Container.Resolve<IAuthenticationService>().GetAllianceRights();
            var isAuthorized = true;

            if (!allianceRights.IsInAlliance) {
                isAuthorized = false;
            }

            if (CanInvite && !allianceRights.CanInvite) {
                isAuthorized = false;
            }

            if (!isAuthorized) {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
        }
    }
}