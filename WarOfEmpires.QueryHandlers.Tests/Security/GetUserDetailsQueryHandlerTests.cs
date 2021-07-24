using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.QueryHandlers.Security;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Tests.Security {
    [TestClass]
    public sealed class GetUserDetailsQueryHandlerTests {
        [TestMethod]
        public void GetUserDetailsQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .BuildAlliance(14)
                .BuildMember(2, lastOnline: new DateTime(2021, 4, 5));

            builder.Player.User.IsAdmin.Returns(true);

            var handler = new GetUserDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetUserDetailsQuery("test1@test.com", 2);

            var result = handler.Execute(query);

            result.Id.Should().Be(2);
            result.Email.Should().Be("test2@test.com");
            result.DisplayName.Should().Be("Test display name 2");
            result.AllianceCode.Should().Be("FS");
            result.AllianceName.Should().Be("Føroyskir Samgonga");
            result.Status.Should().Be("Active");
            result.IsAdmin.Should().BeTrue();
            result.LastOnline.Should().Be(new DateTime(2021, 4, 5));
        }

        [TestMethod]
        public void GetUserDetailsQueryHandler_Throws_Exception_For_Nonexistent_Id() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new GetUserDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetUserDetailsQuery("test1@test.com", 5);

            Action action = () => handler.Execute(query);

            action.Should().Throw<InvalidOperationException>();
        }
    }
}
