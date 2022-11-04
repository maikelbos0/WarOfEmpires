using Microsoft.AspNetCore.Mvc;
using WarOfEmpires.Api.Extensions;
using WarOfEmpires.Api.Services;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Models.Security;

namespace WarOfEmpires.Api.Controllers;

[ApiController]
[Route(nameof(Routing.Security))]
public class SecurityController : ControllerBase {
    private readonly IMessageService messageService;

    public SecurityController(IMessageService messageService) {
        this.messageService = messageService;
    }

    [HttpPost(nameof(Routing.Security.Register))]
    public IActionResult Register(RegisterUserModel model) {
        // TODO centralize
        // TODO warnings?
        if (ModelState.IsValid) {
            var result = messageService.Dispatch(new RegisterUserCommand(model.Email, model.Password));
            ModelState.Merge(result);
        }

        if (ModelState.IsValid) {
            return Ok();
        }
        else {
            return BadRequest(ModelState);
        }
    }
}
