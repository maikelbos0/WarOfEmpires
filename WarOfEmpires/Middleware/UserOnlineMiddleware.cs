using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Extensions;
using WarOfEmpires.Services;

namespace WarOfEmpires.Middleware {
    public class UserOnlineMiddleware {
        private readonly RequestDelegate _next;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMessageService _messageService;

        public UserOnlineMiddleware(RequestDelegate next, IAuthenticationService authenticationService, IMessageService messageService) {
            _next = next;
            _authenticationService = authenticationService;
            _messageService = messageService;
        }

        public async Task InvokeAsync(HttpContext context) {
            await _next(context);

            if (_authenticationService.IsAuthenticated && !(context.Request.Method == "GET" && context.Request.IsAjaxRequest())) {
                var command = new UpdateUserLastOnlineCommand(_authenticationService.Identity);

                _messageService.Dispatch(command);
            }
        }
    }
}
