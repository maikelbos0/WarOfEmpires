using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.Api.Controllers;
using WarOfEmpires.Api.Services;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;

namespace WarOfEmpires.Api.Tests.Controllers;

[TestClass]
public class EmpireControllerTests {
    [TestMethod]
    public void EmpireController_ResourceHeader_Succeeds() {
        var messageService = Substitute.For<IMessageService>();
        var identityService = Substitute.For<IIdentityService>();
        var controller = new EmpireController(messageService, identityService);
        var model = new ResourceHeaderViewModel();

        messageService.Dispatch(Arg.Any<GetResourceHeaderQuery>()).Returns(model);
        identityService.Identity.Returns("test@test.com");

        var response = controller.ResourceHeader();

        response.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(model);

        messageService.Received().Dispatch(Arg.Is<GetResourceHeaderQuery>(c => c.Email == "test@test.com"));
    }
}
