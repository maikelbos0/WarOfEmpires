using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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

    public SecurityController(IMessageService messageService, IIdentityService identityService, ITokenService tokenService) : base(messageService, identityService) {
        this.tokenService = tokenService;
    }

    [AllowAnonymous]
    [HttpPost(nameof(Routing.Security.Register))]
    public IActionResult Register(RegisterUserModel model)
        => ExecuteCommand(new RegisterUserCommand(model.Email, model.Password));

    [AllowAnonymous]
    [HttpPost(nameof(Routing.Security.Activate))]
    public IActionResult Activate(ActivateUserModel model)
        => ExecuteCommand(new ActivateUserCommand(model.Email, model.ActivationCode));

    [AllowAnonymous]
    [HttpPost(nameof(Routing.Security.SendActivation))]
    public IActionResult SendActivation(SendUserActivationModel model)
        => ExecuteCommand(new SendUserActivationCommand(model.Email));

    [AllowAnonymous]
    [HttpPost(nameof(Routing.Security.ForgotPassword))]
    public IActionResult ForgotPassword(ForgotUserPasswordModel model)
        => ExecuteCommand(new ForgotUserPasswordCommand(model.Email));

    [AllowAnonymous]
    [HttpPost(nameof(Routing.Security.ResetPassword))]
    public IActionResult ResetPassword(ResetUserPasswordModel model)
        => ExecuteCommand(new ResetUserPasswordCommand(model.Email, model.PasswordResetToken, model.NewPassword));

    [AllowAnonymous]
    [HttpPost(nameof(Routing.Security.LogIn))]
    public IActionResult LogIn(LogInUserModel model) {
        var result = messageService.Dispatch(new LogInUserCommand(model.Email, model.Password));

        ModelState.Merge(result);

        if (ModelState.IsValid) {
            var requestId = Guid.NewGuid();
            var tokenResult = messageService.Dispatch(new GenerateUserRefreshTokenCommand(model.Email, requestId));

            if (!tokenResult.Success) {
                throw new InvalidOperationException("Failed to generate token");
            }

            var viewModel = messageService.Dispatch(new GetUserClaimsQuery(model.Email, requestId));

            return Ok(new UserTokenModel() {
                AccessToken = tokenService.CreateToken(viewModel),
                RefreshToken = viewModel.RefreshToken
            });
        }
        else {
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
    }

    [AllowAnonymous]
    [HttpPost(nameof(Routing.Security.AcquireToken))]
    public IActionResult AcquireToken(UserTokenModel model) {
        if (tokenService.TryGetIdentity(model.AccessToken, out var email)) {
            var requestId = Guid.NewGuid();
            var tokenResult = messageService.Dispatch(new RotateUserRefreshTokenCommand(email, requestId, model.RefreshToken));

            if (!tokenResult.Success) {
                throw new InvalidOperationException("Failed to generate token");
            }

            var viewModel = messageService.Dispatch(new GetUserClaimsQuery(email, requestId));

            return Ok(new UserTokenModel() {
                AccessToken = tokenService.CreateToken(viewModel),
                RefreshToken = viewModel.RefreshToken
            });
        }
        else {
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
    }

    [HttpPost(nameof(Routing.Security.ChangeEmail))]
    public IActionResult ChangeEmail(ChangeUserEmailModel model)
        => ExecuteCommand(new ChangeUserEmailCommand(identityService.Identity, model.Password, model.NewEmail));

    [AllowAnonymous]
    [HttpPost(nameof(Routing.Security.ConfirmEmail))]
    public IActionResult ConfirmEmail(ConfirmUserEmailChangeModel model)
        => ExecuteCommand(new ConfirmUserEmailChangeCommand(model.Email, model.ConfirmationCode));

    [HttpPost(nameof(Routing.Security.ChangePassword))]
    public IActionResult ChangePassword(ChangeUserPasswordModel model)
        => ExecuteCommand(new ChangeUserPasswordCommand(identityService.Identity, model.CurrentPassword, model.NewPassword));

    [HttpPost(nameof(Routing.Security.Deactivate))]
    public IActionResult Deactivate(DeactivateUserModel model)
        => ExecuteCommand(new DeactivateUserCommand(identityService.Identity, model.Password));
}
