using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Alliances;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Alliances {
    [TestClass]
    public sealed class GetRolesQueryHandlerTests {
        [TestMethod]
        public void GetRolesQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1, out var manager)
                .WithMember(2, out var secondManager)
                .WithMember(3, out var peasant, rank: 5)
                .WithMember(4, out var inactivePeasant, status: UserStatus.Inactive)
                .WithRole(1, "Manager", manager, secondManager)
                .WithRole(2, "Peasant", peasant, inactivePeasant);

            var handler = new GetRolesQueryHandler(builder.Context);
            var query = new GetRolesQuery("test3@test.com");

            var result = handler.Execute(query);

            result.Should().HaveCount(2);

            result.Should().ContainSingle(r => r.Id == 1);
            result.Single(r => r.Id == 1).Name.Should().Be("Manager");
            result.Single(r => r.Id == 1).CanInvite.Should().BeTrue();
            result.Single(r => r.Id == 1).Players.Should().Be(2);

            result.Should().ContainSingle(r => r.Id == 2);
            result.Single(r => r.Id == 2).Name.Should().Be("Peasant");
            result.Single(r => r.Id == 2).CanInvite.Should().BeTrue();
            result.Single(r => r.Id == 2).Players.Should().Be(1);
        }
    }
}