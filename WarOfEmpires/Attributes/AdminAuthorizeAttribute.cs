using System.Web.Mvc;
using WarOfEmpires.Services;
using Unity;
using System.Net;

namespace WarOfEmpires.Attributes {
    sealed public class AdminAuthorizeAttribute : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            if (!UnityConfig.Container.Resolve<IAuthenticationService>().IsAdmin()) {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
        }
    }
}