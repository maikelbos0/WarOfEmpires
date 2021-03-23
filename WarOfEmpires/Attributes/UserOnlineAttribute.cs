using WarOfEmpires.Commands.Security;
using WarOfEmpires.Services;
using Unity;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WarOfEmpires.Attributes {
    public class UserOnlineAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext) {
            var authenticationService = UnityConfig.Container.Resolve<IAuthenticationService>();

            if (authenticationService.IsAuthenticated) {
                var messageService = UnityConfig.Container.Resolve<IMessageService>();
                var command = new UpdateUserLastOnlineCommand(authenticationService.Identity);

                messageService.Dispatch(command);
            }
        }
    }
}