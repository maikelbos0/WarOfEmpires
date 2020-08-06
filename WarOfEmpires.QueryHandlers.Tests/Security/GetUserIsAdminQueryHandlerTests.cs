using WarOfEmpires.Queries.Security;
using WarOfEmpires.QueryHandlers.Security;
using WarOfEmpires.Test.Utilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace WarOfEmpires.QueryHandlers.Tests.Security {
    [TestClass]
    public sealed class GetUserIsAdminQueryHandlerTests {
        [TestMethod]
        public void GetUserIsAdminQueryHandler_Returns_True_For_Admin() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            builder.User.IsAdmin.Returns(true);

            var handler = new GetUserIsAdminQueryHandler(builder.Context);
            var query = new GetUserIsAdminQuery("test1@test.com");

            var result = handler.Execute(query);

            result.Should().BeTrue();
        }

        [TestMethod]
        public void GetUserIsAdminQueryHandler_Returns_False_For_Normal_User() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            builder.User.IsAdmin.Returns(false);
            
            var handler = new GetUserIsAdminQueryHandler(builder.Context);
            var query = new GetUserIsAdminQuery("test1@test.com");
            
            var result = handler.Execute(query);

            result.Should().BeFalse();
        }
    }
}