using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.QueryHandlers.Security;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Tests.Security {
    [TestClass]
    public sealed class GetUsersQueryHandlerTests {
        [TestMethod]
        public void GetUsersQueryHandler_Returns_All_Users() {
            var builder = new FakeBuilder()
                .WithPlayer(1, status: UserStatus.New)
                .WithPlayer(2, status: UserStatus.Inactive)
                .WithPlayer(3, status: UserStatus.Active);

            var handler = new GetUsersQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetUsersQuery(null);

            var result = handler.Execute(query);

            result.Should().HaveCount(3);
        }

        [TestMethod]
        public void GetUsersQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .BuildMember(2, lastOnline: new DateTime(2021, 2, 3, 14, 30, 45));

            builder.User.IsAdmin.Returns(true);

            var handler = new GetUsersQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetUsersQuery(null);

            var result = handler.Execute(query);

            result.Should().HaveCount(1);
            result.Single().Id.Should().Be(2);
            result.Single().Email.Should().Be("test2@test.com");
            result.Single().DisplayName.Should().Be("Test display name 2");
            result.Single().AllianceCode.Should().Be("FS");
            result.Single().AllianceName.Should().Be("Føroyskir Samgonga");
            result.Single().Status.Should().Be("Active");
            result.Single().IsAdmin.Should().BeTrue();
            result.Single().LastOnline.Should().Be(new DateTime(2021, 2, 3, 14, 30, 45));
        }

        [TestMethod]
        public void GetUsersQueryHandler_Searches() {
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .WithPlayer(2, displayName: "Wrong");

            var handler = new GetUsersQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetUsersQuery("Test");

            var result = handler.Execute(query);

            result.Should().HaveCount(1);
            result.Single().DisplayName.Should().Be("Test display name 1");
        }
    }
}
