using Microsoft.AspNetCore.Mvc.Filters;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Services;

namespace WarOfEmpires.Filters {
    public class UserOnlineFilter : IActionFilter {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMessageService _messageService;

        public UserOnlineFilter(IAuthenticationService authenticationService, IMessageService messageService) {
            _authenticationService = authenticationService;
            _messageService = messageService;
        }

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context) {
            if (_authenticationService.IsAuthenticated) {
                var command = new UpdateUserLastOnlineCommand(_authenticationService.Identity);

                _messageService.Dispatch(command);
            }
        }
    }
}
