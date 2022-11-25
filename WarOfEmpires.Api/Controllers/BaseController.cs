using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarOfEmpires.Api.Extensions;
using WarOfEmpires.Api.Services;
using WarOfEmpires.Commands;

namespace WarOfEmpires.Api.Controllers {
    [Authorize]
    public abstract class BaseController : ControllerBase {
        protected readonly IMessageService messageService;
        protected readonly IIdentityService identityService;

        public BaseController(IMessageService messageService, IIdentityService identityService) {
            this.messageService = messageService;
            this.identityService = identityService;
        }

        protected IActionResult ExecuteCommand<TCommand>(TCommand command) where TCommand : ICommand {
            var result = messageService.Dispatch(command);

            ModelState.Merge(result);

            if (ModelState.IsValid) {
                return Ok(result.Warnings);
            }
            else {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
        }
    }
}