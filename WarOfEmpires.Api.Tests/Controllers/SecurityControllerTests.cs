using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.Api.Controllers;
using WarOfEmpires.Api.Services;
using WarOfEmpires.CommandHandlers;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Models.Security;

namespace WarOfEmpires.Api.Tests.Controllers;

[TestClass]
public class SecurityControllerTests {
    [TestMethod]
    public void SecurityController_Register_Succeeds() {
        var messageService = Substitute.For<IMessageService>();
        var controller = new SecurityController(messageService, Substitute.For<IIdentityService>(), Substitute.For<ITokenService>());

        messageService.Dispatch(Arg.Any<RegisterUserCommand>()).Returns(new CommandResult<RegisterUserCommand>());

        var response = controller.Register(new RegisterUserModel() { Email = "test@test.com", Password = "password" });

        Assert.IsInstanceOfType(response, typeof(OkObjectResult));

        messageService.Received().Dispatch(Arg.Is<RegisterUserCommand>(c => c.Email == "test@test.com" && c.Password == "password"));
    }

    [TestMethod]
    public void SecurityController_Activate_Succeeds() {
        var messageService = Substitute.For<IMessageService>();
        var controller = new SecurityController(messageService, Substitute.For<IIdentityService>(), Substitute.For<ITokenService>());

        messageService.Dispatch(Arg.Any<ActivateUserCommand>()).Returns(new CommandResult<ActivateUserCommand>());

        var response = controller.Activate(new ActivateUserModel() { Email = "test@test.com", ActivationCode = "12345" });

        Assert.IsInstanceOfType(response, typeof(OkObjectResult));

        messageService.Received().Dispatch(Arg.Is<ActivateUserCommand>(c => c.Email == "test@test.com" && c.ActivationCode == "12345"));
    }

    [TestMethod]
    public void SecurityController_SendActivation_Succeeds() {
        var messageService = Substitute.For<IMessageService>();
        var controller = new SecurityController(messageService, Substitute.For<IIdentityService>(), Substitute.For<ITokenService>());

        messageService.Dispatch(Arg.Any<SendUserActivationCommand>()).Returns(new CommandResult<SendUserActivationCommand>());

        var response = controller.SendActivation(new SendUserActivationModel() { Email = "test@test.com" });

        Assert.IsInstanceOfType(response, typeof(OkObjectResult));

        messageService.Received().Dispatch(Arg.Is<SendUserActivationCommand>(c => c.Email == "test@test.com"));
    }

    [TestMethod]
    public void SecurityController_ForgotPassword_Succeeds() {
        var messageService = Substitute.For<IMessageService>();
        var controller = new SecurityController(messageService, Substitute.For<IIdentityService>(), Substitute.For<ITokenService>());

        messageService.Dispatch(Arg.Any<ForgotUserPasswordCommand>()).Returns(new CommandResult<ForgotUserPasswordCommand>());

        var response = controller.ForgotPassword(new ForgotUserPasswordModel() { Email = "test@test.com" });

        Assert.IsInstanceOfType(response, typeof(OkObjectResult));

        messageService.Received().Dispatch(Arg.Is<ForgotUserPasswordCommand>(c => c.Email == "test@test.com"));
    }

    [TestMethod]
    public void SecurityController_ResetPassword_Succeeds() {
        var messageService = Substitute.For<IMessageService>();
        var controller = new SecurityController(messageService, Substitute.For<IIdentityService>(), Substitute.For<ITokenService>());

        messageService.Dispatch(Arg.Any<ResetUserPasswordCommand>()).Returns(new CommandResult<ResetUserPasswordCommand>());

        var response = controller.ResetPassword(new ResetUserPasswordModel() { Email = "test@test.com", NewPassword = "password", PasswordResetToken = "12345" });

        Assert.IsInstanceOfType(response, typeof(OkObjectResult));

        messageService.Received().Dispatch(Arg.Is<ResetUserPasswordCommand>(c => c.Email == "test@test.com" && c.NewPassword == "password" && c.PasswordResetToken == "12345"));
    }

    [TestMethod]
    public void SecurityController_ChangeEmail_Succeeds() {
        var messageService = Substitute.For<IMessageService>();
        var identityService = Substitute.For<IIdentityService>();
        var controller = new SecurityController(messageService, identityService, Substitute.For<ITokenService>());

        messageService.Dispatch(Arg.Any<ChangeUserEmailCommand>()).Returns(new CommandResult<ChangeUserEmailCommand>());
        identityService.Identity.Returns("test@test.com");

        var response = controller.ChangeEmail(new ChangeUserEmailModel() { Password = "password", NewEmail = "new@test.com" });

        Assert.IsInstanceOfType(response, typeof(OkObjectResult));

        messageService.Received().Dispatch(Arg.Is<ChangeUserEmailCommand>(c => c.CurrentEmail == "test@test.com" && c.Password == "password" && c.NewEmail == "new@test.com"));
    }

    [TestMethod]
    public void SecurityController_ConfirmEmail_Succeeds() {
        var messageService = Substitute.For<IMessageService>();
        var controller = new SecurityController(messageService, Substitute.For<IIdentityService>(), Substitute.For<ITokenService>());

        messageService.Dispatch(Arg.Any<ConfirmUserEmailChangeCommand>()).Returns(new CommandResult<ConfirmUserEmailChangeCommand>());

        var response = controller.ConfirmEmail(new ConfirmUserEmailChangeModel() { Email = "test@test.com", ConfirmationCode = "12345" });

        Assert.IsInstanceOfType(response, typeof(OkObjectResult));

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

        Assert.IsInstanceOfType(response, typeof(OkObjectResult));

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

        Assert.IsInstanceOfType(response, typeof(OkObjectResult));

        messageService.Received().Dispatch(Arg.Is<DeactivateUserCommand>(c => c.Email == "test@test.com" && c.Password == "password"));
    }
}
