using System.Net;
using System.Web.Mvc;
using Unity;
using WarOfEmpires.Services;

namespace WarOfEmpires.Attributes {
    public sealed class AdminAuthorizeAttribute : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            if (!UnityConfig.Container.Resolve<IAuthorizationService>().IsAdmin()) {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
        }
    }
}