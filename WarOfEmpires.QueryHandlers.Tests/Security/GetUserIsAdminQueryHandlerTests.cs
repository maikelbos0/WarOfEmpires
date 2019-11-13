using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.QueryHandlers.Security;
using WarOfEmpires.Test.Utilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace WarOfEmpires.QueryHandlers.Tests.Security {
    [TestClass]
    public sealed class GetUserIsAdminQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        [TestMethod]
        public void GetUserIsAdminQueryHandler_Returns_True_For_Admin() {
            var handler = new GetUserIsAdminQueryHandler(_context);
            var query = new GetUserIsAdminQuery("test@test.com");
            var user = Substitute.For<User>();

            user.Email.Returns("test@test.com");
            user.IsAdmin.Returns(true);
            
            _context.Users.Add(user);

            var result = handler.Execute(query);

            result.Should().BeTrue();
        }

        [TestMethod]
        public void GetUserIsAdminQueryHandler_Returns_False_For_Normal_User() {
            var handler = new GetUserIsAdminQueryHandler(_context);
            var query = new GetUserIsAdminQuery("test@test.com");
            var user = Substitute.For<User>();

            user.Email.Returns("test@test.com");
            user.IsAdmin.Returns(false);

            _context.Users.Add(user);

            var result = handler.Execute(query);

            result.Should().BeFalse();
        }
    }
}