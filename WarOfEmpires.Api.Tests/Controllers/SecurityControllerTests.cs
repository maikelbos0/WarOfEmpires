using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.Api.Controllers;
using WarOfEmpires.Api.Services;
using WarOfEmpires.CommandHandlers;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Models.Security;
using WarOfEmpires.Queries.Security;

namespace WarOfEmpires.Api.Tests.Controllers;

[TestClass]
public class SecurityControllerTests {
    [TestMethod]
    public void SecurityController_Register_Succeeds() {
        var messageService = Substitute.For<IMessageService>();
        var controller = new SecurityController(messageService, Substitute.For<IIdentityService>(), Substitute.For<ITokenService>());

        messageService.Dispatch(Arg.Any<RegisterUserCommand>()).Returns(new CommandResult<RegisterUserCommand>());

        var response = controller.Register(new RegisterUserModel() { Email = "test@test.com", Password = "password" });

        response.Should().BeOfType<OkObjectResult>();

        messageService.Received().Dispatch(Arg.Is<RegisterUserCommand>(c => c.Email == "test@test.com" && c.Password == "password"));
    }

    [TestMethod]
    public void SecurityController_Activate_Succeeds() {
        var messageService = Substitute.For<IMessageService>();
        var controller = new SecurityController(messageService, Substitute.For<IIdentityService>(), Substitute.For<ITokenService>());

        messageService.Dispatch(Arg.Any<ActivateUserCommand>()).Returns(new CommandResult<ActivateUserCommand>());

        var response = controller.Activate(new ActivateUserModel() { Email = "test@test.com", ActivationCode = "12345" });

        response.Should().BeOfType<OkObjectResult>();

        messageService.Received().Dispatch(Arg.Is<ActivateUserCommand>(c => c.Email == "test@test.com" && c.ActivationCode == "12345"));
    }

    [TestMethod]
    public void SecurityController_SendActivation_Succeeds() {
        var messageService = Substitute.For<IMessageService>();
        var controller = new SecurityController(messageService, Substitute.For<IIdentityService>(), Substitute.For<ITokenService>());

        messageService.Dispatch(Arg.Any<SendUserActivationCommand>()).Returns(new CommandResult<SendUserActivationCommand>());

        var response = controller.SendActivation(new SendUserActivationModel() { Email = "test@test.com" });

        response.Should().BeOfType<OkObjectResult>();

        messageService.Received().Dispatch(Arg.Is<SendUserActivationCommand>(c => c.Email == "test@test.com"));
    }

    [TestMethod]
    public void SecurityController_ForgotPassword_Succeeds() {
        var messageService = Substitute.For<IMessageService>();
        var controller = new SecurityController(messageService, Substitute.For<IIdentityService>(), Substitute.For<ITokenService>());

        messageService.Dispatch(Arg.Any<ForgotUserPasswordCommand>()).Returns(new CommandResult<ForgotUserPasswordCommand>());

        var response = controller.ForgotPassword(new ForgotUserPasswordModel() { Email = "test@test.com" });

        response.Should().BeOfType<OkObjectResult>();

        messageService.Received().Dispatch(Arg.Is<ForgotUserPasswordCommand>(c => c.Email == "test@test.com"));
    }

    [TestMethod]
    public void SecurityController_ResetPassword_Succeeds() {
        var messageService = Substitute.For<IMessageService>();
        var controller = new SecurityController(messageService, Substitute.For<IIdentityService>(), Substitute.For<ITokenService>());

        messageService.Dispatch(Arg.Any<ResetUserPasswordCommand>()).Returns(new CommandResult<ResetUserPasswordCommand>());

        var response = controller.ResetPassword(new ResetUserPasswordModel() { Email = "test@test.com", NewPassword = "password", PasswordResetToken = "12345" });

        response.Should().BeOfType<OkObjectResult>();

        messageService.Received().Dispatch(Arg.Is<ResetUserPasswordCommand>(c => c.Email == "test@test.com" && c.NewPassword == "password" && c.PasswordResetToken == "12345"));
    }

    [TestMethod]
    public void SecurityController_LogIn_Succeeds() {
        var messageService = Substitute.For<IMessageService>();
        var tokenService = Substitute.For<ITokenService>();
        var controller = new SecurityController(messageService, Substitute.For<IIdentityService>(), tokenService);
        var requestId = Guid.Empty;

        messageService.Dispatch(Arg.Any<LogInUserCommand>()).Returns(new CommandResult<LogInUserCommand>());
        messageService.Dispatch(Arg.Do<GenerateUserRefreshTokenCommand>(c => requestId = c.RequestId)).Returns(new CommandResult<GenerateUserRefreshTokenCommand>());
        messageService.Dispatch(Arg.Any<GetUserClaimsQuery>()).Returns(new UserClaimsViewModel() { RefreshToken = "12345" });
        tokenService.CreateToken(Arg.Any<UserClaimsViewModel>()).Returns("token");

        var response = controller.LogIn(new LogInUserModel() { Email = "test@test.com", Password = "password" });

        var model = response.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeOfType<UserTokenModel>().Subject;
        model.AccessToken.Should().Be("token");
        model.RefreshToken.Should().Be("12345");

        Received.InOrder(() => {
            messageService.Dispatch(Arg.Is<LogInUserCommand>(c => c.Email == "test@test.com" && c.Password == "password"));
            messageService.Dispatch(Arg.Is<GenerateUserRefreshTokenCommand>(c => c.Email == "test@test.com"));
            messageService.Dispatch(Arg.Is<GetUserClaimsQuery>(c => c.Email == "test@test.com" && c.RequestId == requestId));
            tokenService.CreateToken(Arg.Any<UserClaimsViewModel>());
        });
    }
    
    [TestMethod]
    public void SecurityController_AcquireNewTokens_Succeeds() {
        var messageService = Substitute.For<IMessageService>();
        var tokenService = Substitute.For<ITokenService>();
        var controller = new SecurityController(messageService, Substitute.For<IIdentityService>(), tokenService);
        var requestId = Guid.Empty;

        messageService.Dispatch(Arg.Do<RotateUserRefreshTokenCommand>(c => requestId = c.RequestId)).Returns(new CommandResult<RotateUserRefreshTokenCommand>());
        messageService.Dispatch(Arg.Any<GetUserClaimsQuery>()).Returns(new UserClaimsViewModel() { RefreshToken = "12345" });
        tokenService.TryGetIdentity(Arg.Any<string>(), out Arg.Any<string>()).Returns(x => { x[1] = "test@test.com"; return true; });
        tokenService.CreateToken(Arg.Any<UserClaimsViewModel>()).Returns("token");

        var response = controller.AcquireNewTokens(new UserTokenModel() { AccessToken = "old", RefreshToken = "01234" });

        var model = response.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeOfType<UserTokenModel>().Subject;
        model.AccessToken.Should().Be("token");
        model.RefreshToken.Should().Be("12345");

        Received.InOrder(() => {
            tokenService.TryGetIdentity("old", out Arg.Any<string>());
            messageService.Dispatch(Arg.Is<RotateUserRefreshTokenCommand>(c => c.Email == "test@test.com" && c.RefreshToken == "01234"));
            messageService.Dispatch(Arg.Is<GetUserClaimsQuery>(c => c.Email == "test@test.com" && c.RequestId == requestId));
            tokenService.CreateToken(Arg.Any<UserClaimsViewModel>());
        });
    }

    [TestMethod]
    public void SecurityController_ChangeEmail_Succeeds() {
        var messageService = Substitute.For<IMessageService>();
        var identityService = Substitute.For<IIdentityService>();
        var controller = new SecurityController(messageService, identityService, Substitute.For<ITokenService>());

        messageService.Dispatch(Arg.Any<ChangeUserEmailCommand>()).Returns(new CommandResult<ChangeUserEmailCommand>());
        identityService.Identity.Returns("test@test.com");

        var response = controller.ChangeEmail(new ChangeUserEmailModel() { Password = "password", NewEmail = "new@test.com" });

        response.Should().BeOfType<OkObjectResult>();

        messageService.Received().Dispatch(Arg.Is<ChangeUserEmailCommand>(c => c.CurrentEmail == "test@test.com" && c.Password == "password" && c.NewEmail == "new@test.com"));
    }

    [TestMethod]
    public void SecurityController_ConfirmEmail_Succeeds() {
        var messageService = Substitute.For<IMessageService>();
        var controller = new SecurityController(messageService, Substitute.For<IIdentityService>(), Substitute.For<ITokenService>());

        messageService.Dispatch(Arg.Any<ConfirmUserEmailChangeCommand>()).Returns(new CommandResult<ConfirmUserEmailChangeCommand>());

        var response = controller.ConfirmEmail(new ConfirmUserEmailChangeModel() { Email = "test@test.com", ConfirmationCode = "12345" });

        response.Should().BeOfType<OkObjectResult>();

        messageService.Received().Dispatch(Arg.Is<ConfirmUserEmailChangeCommand>(c => c.Email == "test@test.com" && c.ConfirmationCode == "12345"));
    }

    [TestMethod]
    public void SecurityController_ChangePassword_Succeeds() {
        var messageService = Substitute.For<IMessageService>();
        var identityService = Substitute.For<IIdentityService>();
        var controller = new SecurityController(messageService, identityService, Substitute.For<ITokenService>());

        messageService.Dispatch(Arg.Any<ChangeUserPasswordCommand>()).Returns(new CommandResult<ChangeUserPasswordCommand>());
        identityService.Identity.Returns("test@test.com");

        var response = controller.ChangePassword(new ChangeUserPasswordModel() { CurrentPassword = "old", NewPassword = "password" });

        response.Should().BeOfType<OkObjectResult>();

        messageService.Received().Dispatch(Arg.Is<ChangeUserPasswordCommand>(c => c.Email == "test@test.com" && c.CurrentPassword == "old" && c.NewPassword == "password"));
    }

    [TestMethod]
    public void SecurityController_Deactivate_Succeeds() {
        var messageService = Substitute.For<IMessageService>();
        var identityService = Substitute.For<IIdentityService>();
        var controller = new SecurityController(messageService, identityService, Substitute.For<ITokenService>());

        messageService.Dispatch(Arg.Any<DeactivateUserCommand>()).Returns(new CommandResult<DeactivateUserCommand>());
        identityService.Identity.Returns("test@test.com");

        var response = controller.Deactivate(new DeactivateUserModel() { Password = "password" });

        response.Should().BeOfType<OkObjectResult>();

        messageService.Received().Dispatch(Arg.Is<DeactivateUserCommand>(c => c.Email == "test@test.com" && c.Password == "password"));
    }
}
