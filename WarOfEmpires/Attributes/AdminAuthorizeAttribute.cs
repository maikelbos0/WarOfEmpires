using System.Net;
using WarOfEmpires.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace WarOfEmpires.Attributes {
    // TODO consider moving away from ActionFilterAttribute
    public sealed class AdminAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            // TODO determine if this is still the way to do custom authorization
            if (!filterContext.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>().IsAdmin()) {
                filterContext.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
            }
        }
    }
}