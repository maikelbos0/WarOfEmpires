using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Security.Claims;
using System.Security.Principal;
using WarOfEmpires.Api.Services;

namespace WarOfEmpires.Api.Tests.Services;

[TestClass]
public class IdentityServiceTests {
    [DataTestMethod]
    [DataRow(true, true)]
    [DataRow(false, false)]
    public void IdentityService_IsAuthenticated_Returns_Correct_Value(bool identityIsAuthenticated, bool expectedIsAuthenticated) {
        var contextAccessor = Substitute.For<IHttpContextAccessor>();
        var context = Substitute.For<HttpContext>();
        var user = Substitute.For<ClaimsPrincipal>();
        var identity = Substitute.For<IIdentity>();
        var service = new IdentityService(contextAccessor);

        contextAccessor.HttpContext.Returns(context);
        context.User = user;
        user.Identity.Returns(identity);
        identity.IsAuthenticated.Returns(identityIsAuthenticated);

        service.IsAuthenticated.Should().Be(expectedIsAuthenticated);
    }

    [TestMethod]
    public void IdentityService_IsAuthenticated_Returns_False_For_Missing_Identity() {
        var contextAccessor = Substitute.For<IHttpContextAccessor>();
        var context = Substitute.For<HttpContext>();
        var user = Substitute.For<ClaimsPrincipal>();
        var service = new IdentityService(contextAccessor);

        contextAccessor.HttpContext.Returns(context);
        context.User = user;
        user.Identity.Returns((IIdentity)null);

        service.IsAuthenticated.Should().BeFalse();
    }

    [TestMethod]
    public void IdentityService_IsAuthenticated_Throws_Exception_For_Missing_HttpContext() {
        var contextAccessor = Substitute.For<IHttpContextAccessor>();
        var service = new IdentityService(contextAccessor);

        contextAccessor.HttpContext.Returns((HttpContext)null);

        Action action = () => { var isAuthenticated = service.IsAuthenticated; };
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void IdentityService_Identity_Returns_Correct_Value() {
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
    public void IdentityService_Identity_Throws_Exception_For_Missing_HttpContext() {
        var contextAccessor = Substitute.For<IHttpContextAccessor>();
        var service = new IdentityService(contextAccessor);

        contextAccessor.HttpContext.Returns((HttpContext)null);

        Action action = () => { var identity = service.Identity; };
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void IdentityService_Identity_Throws_Exception_For_Missing_Subject() {
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
