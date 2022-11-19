using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Security.Claims;
using WarOfEmpires.Api.Services;

namespace WarOfEmpires.Api.Tests.Services;

[TestClass]
public class IdentityServiceTests {
    [TestMethod]
    public void Identity() {
        var contextAccessor = Substitute.For<IHttpContextAccessor>();
        var context = Substitute.For<HttpContext>();
        var user = Substitute.For<ClaimsPrincipal>();
        var service = new IdentityService(contextAccessor);

        contextAccessor.HttpContext.Returns(context);
        context.User = user;
        user.Claims.Returns(new Claim[] {
            new Claim(ClaimTypes.Name, "Test"),
            new Claim(ClaimTypes.NameIdentifier, "test@test.com")
        });

        service.Identity.Should().Be("test@test.com");
    }

    [TestMethod]
    public void Identity_Throws_Exception_For_Missing_HttpContext() {
        var contextAccessor = Substitute.For<IHttpContextAccessor>();
        var service = new IdentityService(contextAccessor);

        contextAccessor.HttpContext.Returns((HttpContext)null);

        Action action = () => { var identity = service.Identity; };
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void Identity_Throws_Exception_For_Missing_Subject() {
        var contextAccessor = Substitute.For<IHttpContextAccessor>();
        var context = Substitute.For<HttpContext>();
        var user = Substitute.For<ClaimsPrincipal>();
        var service = new IdentityService(contextAccessor);

        contextAccessor.HttpContext.Returns(context);
        context.User = user;
        user.Claims.Returns(new Claim[] {
            new Claim(ClaimTypes.Name, "Test")
        });

        Action action = () => { var identity = service.Identity; };
        action.Should().Throw<InvalidOperationException>();
    }
}
