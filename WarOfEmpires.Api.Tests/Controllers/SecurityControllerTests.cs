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
    public void SecurityController_SendActivation_Succeeds() {
        var messageService = Substitute.For<IMessageService>();
        var controller = new SecurityController(messageService, Substitute.For<IIdentityService>(), Substitute.For<ITokenService>());

        messageService.Dispatch(Arg.Any<SendUserActivationCommand>()).Returns(new CommandResult<SendUserActivationCommand>());

        var response = controller.SendActivation(new SendUserActivationModel() { Email = "test@test.com" });

        Assert.IsInstanceOfType(response, typeof(OkObjectResult));

        messageService.Received().Dispatch(Arg.Is<SendUserActivationCommand>(c => c.Email == "test@test.com"));
    }
}
