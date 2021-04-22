using WarOfEmpires.Commands.Security;
using WarOfEmpires.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace WarOfEmpires.Attributes {
    public class UserOnlineAttribute : ActionFilterAttribute
    {
        // TODO consider moving away from ActionFilterAttribute
        public override void OnActionExecuted(ActionExecutedContext filterContext) {
            var authenticationService = filterContext.HttpContext.RequestServices.GetRequiredService<IAuthenticationService>();

            if (authenticationService.IsAuthenticated) {
                var messageService = filterContext.HttpContext.RequestServices.GetRequiredService<IMessageService>();
                var command = new UpdateUserLastOnlineCommand(authenticationService.Identity);

                messageService.Dispatch(command);
            }
        }
    }
}