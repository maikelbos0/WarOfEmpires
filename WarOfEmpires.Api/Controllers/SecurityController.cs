using Microsoft.AspNetCore.Mvc;
using WarOfEmpires.Api.Services;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Models.Security;

namespace WarOfEmpires.Api.Controllers;

[ApiController]
[Route(nameof(Routing.Security))]
public class SecurityController : BaseController {
    public SecurityController(IMessageService messageService) : base(messageService) { }

    [HttpPost(nameof(Routing.Security.Register))]
    public IActionResult Register(RegisterUserModel model)
        => ExecuteCommand(new RegisterUserCommand(model.Email, model.Password));


    [HttpPost(nameof(Routing.Security.Activate))]
    public IActionResult Activate(ActivateUserModel model)
        => ExecuteCommand(new ActivateUserCommand(model.Email, model.ActivationCode));
}
