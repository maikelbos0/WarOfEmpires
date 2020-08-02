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
    public sealed class GetRoleDetailsQueryHandlerTests {
        [TestMethod]
        public void GetRoleDetailsQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder().BuildAlliance(1);

            builder.WithRole(1, "Manager", builder.BuildMember(1).Player, builder.BuildMember(2).Player)
                .WithRole(2, "Peasant", builder.BuildMember(3, rank: 5).Player, builder.BuildMember(4, status: UserStatus.Inactive).Player);

            var handler = new GetRoleDetailsQueryHandler(builder.Context);
            var query = new GetRoleDetailsQuery("test3@test.com", "2");

            var result = handler.Execute(query);

            result.Id.Should().Be(2);
            result.Name.Should().Be("Peasant");
            result.Players.Should().HaveCount(1);

            result.Players.Should().ContainSingle(p => p.Id == 3);
            result.Players.Single(p => p.Id == 3).DisplayName.Should().Be("Test display name 3");
            result.Players.Single(p => p.Id == 3).Rank.Should().Be(5);
        }

        [TestMethod]
        public void GetRoleDetailsQueryHandler_Throws_Exception_For_Role_Of_Different_Alliance() {
            var builder = new FakeBuilder().BuildAlliance(1);

            builder.WithRole(1, "Manager", builder.BuildMember(1).Player, builder.BuildMember(2).Player)
                .WithRole(2, "Peasant", builder.BuildMember(3, rank: 5).Player, builder.BuildMember(4, status: UserStatus.Inactive).Player);

            builder.BuildAlliance(2)
                .BuildMember(5, email: "wrong@test.com");

            var handler = new GetRoleDetailsQueryHandler(builder.Context);
            var query = new GetRoleDetailsQuery("wrong@test.com", "1");

            Action action = () => handler.Execute(query);

            action.Should().Throw<InvalidOperationException>();
        }
    }
}