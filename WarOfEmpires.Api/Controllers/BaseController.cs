using Microsoft.AspNetCore.Mvc;
using WarOfEmpires.Api.Extensions;
using WarOfEmpires.Api.Services;
using WarOfEmpires.Commands;

namespace WarOfEmpires.Api.Controllers {
    public abstract class BaseController : ControllerBase {
        protected readonly IMessageService messageService;

        public BaseController(IMessageService messageService) {
            this.messageService = messageService;
        }

        protected IActionResult ExecuteCommand<TCommand>(TCommand command) where TCommand : ICommand {
            var result = messageService.Dispatch(command);

            ModelState.Merge(result);

            if (ModelState.IsValid) {
                return Ok(result!.Warnings);
            }
            else {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
        }
    }
}