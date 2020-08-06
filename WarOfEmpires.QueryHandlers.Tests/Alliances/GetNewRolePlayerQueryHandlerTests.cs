using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Alliances;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Alliances {
    [TestClass]
    public sealed class GetNewRolePlayerQueryHandlerTests {
        [TestMethod]
        public void GetNewRolePlayerQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1, out var manager)
                .WithMember(2, out var inactiveManager, status: UserStatus.Inactive)
                .WithMember(3, displayName: "Another test display name")
                .WithMember(4, out var peasant)
                .WithRole(1, "Manager", manager, inactiveManager)
                .WithRole(2, "Peasant", peasant);

            var handler = new GetNewRolePlayerQueryHandler(builder.Context);
            var query = new GetNewRolePlayerQuery("test3@test.com", "2");

            var result = handler.Execute(query);

            result.Id.Should().Be(2);
            result.Name.Should().Be("Peasant");
            result.Players.Should().HaveCount(2);

            result.Players.First().Id.Should().Be(3);
            result.Players.First().DisplayName.Should().Be("Another test display name");
        }

        [TestMethod]
        public void GetNewRolePlayerQueryHandler_Throws_Exception_For_Role_Of_Different_Alliance() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithRole(2, "Peasant")
                .BuildAlliance(2)
                .WithMember(5, email: "wrong@test.com");

            var handler = new GetNewRolePlayerQueryHandler(builder.Context);
            var query = new GetNewRolePlayerQuery("wrong@test.com", "2");

            Action action = () => handler.Execute(query);

            action.Should().Throw<InvalidOperationException>();
        }
    }
}