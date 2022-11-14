using Microsoft.AspNetCore.Mvc;
using WarOfEmpires.Api.Extensions;
using WarOfEmpires.Api.Services;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Models.Security;
using WarOfEmpires.Queries.Security;

namespace WarOfEmpires.Api.Controllers;

[ApiController]
[Route(nameof(Routing.Security))]
public sealed class SecurityController : BaseController {
    private readonly ITokenService tokenService;

    public SecurityController(IMessageService messageService, ITokenService tokenService) : base(messageService) {
        this.tokenService = tokenService;
    }

    [HttpPost(nameof(Routing.Security.Register))]
    public IActionResult Register(RegisterUserModel model)
        => ExecuteCommand(new RegisterUserCommand(model.Email, model.Password));

    [HttpPost(nameof(Routing.Security.Activate))]
    public IActionResult Activate(ActivateUserModel model)
        => ExecuteCommand(new ActivateUserCommand(model.Email, model.ActivationCode));

    [HttpPost(nameof(Routing.Security.SendActivation))]
    public IActionResult SendActivation(SendUserActivationModel model)
        => ExecuteCommand(new SendUserActivationCommand(model.Email));

    [HttpPost(nameof(Routing.Security.ForgotPassword))]
    public IActionResult ForgotPassword(ForgotUserPasswordModel model)
        => ExecuteCommand(new ForgotUserPasswordCommand(model.Email));

    [HttpPost(nameof(Routing.Security.LogIn))]
    public IActionResult LogIn(LogInUserModel model) {
        var result = messageService.Dispatch(new LogInUserCommand(model.Email, model.Password));
        ModelState.Merge(result);

        if (ModelState.IsValid) {
            var isAdmin = messageService.Dispatch(new GetUserIsAdminQuery(model.Email));

            return Ok(tokenService.CreateToken(model.Email, isAdmin));
        }
        else {
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
    }
}
