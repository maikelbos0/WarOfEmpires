using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Players {
    [TestClass]
    public sealed class GetBlockedPlayersQueryHandlerTests {
        [TestMethod]
        public void GetBlockedPlayersQueryHandler_Returns_All_Active_BlockedPlayers() {
            var builder = new FakeBuilder()
                .WithPlayer(2, out var inactivePlayer, status: UserStatus.Inactive)
                .WithPlayer(3, out var newPlayer, status: UserStatus.New)
                .WithPlayer(4, out var player1)
                .WithPlayer(5, out var player2)
                .BuildPlayer(1)
                .WithPlayerBlock(1, inactivePlayer)
                .WithPlayerBlock(2, newPlayer)
                .WithPlayerBlock(3, player1)
                .WithPlayerBlock(4, player2);

            var handler = new GetBlockedPlayersQueryHandler(builder.Context);
            var query = new GetBlockedPlayersQuery("test1@test.com");

            var result = handler.Execute(query);

            result.Should().HaveCount(2);
        }

        [TestMethod]
        public void GetBlockedPlayersQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .WithPlayer(2, out var player, displayName: "Blocked")
                .BuildPlayer(1)
                .WithPlayerBlock(1, player);

            var handler = new GetBlockedPlayersQueryHandler(builder.Context);
            var query = new GetBlockedPlayersQuery("test1@test.com");

            var result = handler.Execute(query);

            result.Should().HaveCount(1);
            result.Single().Id.Should().Be(2);
            result.Single().DisplayName.Should().Be("Blocked");
        }
    }
}
