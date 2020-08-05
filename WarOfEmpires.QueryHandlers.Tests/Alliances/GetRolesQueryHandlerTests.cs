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
            var builder = new FakeBuilder().BuildAlliance(1);

            builder.WithRole(1, "Manager", builder.BuildMember(1).Player, builder.BuildMember(2).Player)
                .WithRole(2, "Peasant", builder.BuildMember(3).Player, builder.BuildMember(4, status: UserStatus.Inactive).Player);                

            var handler = new GetRolesQueryHandler(builder.Context);
            var query = new GetRolesQuery("test3@test.com");

            var result = handler.Execute(query);

            result.Should().HaveCount(2);

            result.Should().ContainSingle(r => r.Id == 1);
            result.Single(r => r.Id == 1).Name.Should().Be("Manager");
            result.Single(r => r.Id == 1).Players.Should().Be(2);

            result.Should().ContainSingle(r => r.Id == 2);
            result.Single(r => r.Id == 2).Name.Should().Be("Peasant");
            result.Single(r => r.Id == 2).Players.Should().Be(1);
        }
    }
}