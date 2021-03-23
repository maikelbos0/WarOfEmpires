using System.Net;
using Unity;
using WarOfEmpires.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WarOfEmpires.Attributes {
    public sealed class AdminAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            if (!UnityConfig.Container.Resolve<IAuthorizationService>().IsAdmin()) {
                filterContext.Result = new StatusCodeResult(HttpStatusCode.Forbidden);
            }
        }
    }
}