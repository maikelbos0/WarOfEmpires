using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Game;
using WarOfEmpires.Queries.Game;
using WarOfEmpires.QueryHandlers.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Game {
    [TestClass]
    public sealed class GetGameStatusQueryHandlerTests {
        [TestMethod]
        public void GetGameStatusQueryHandler_Returns_Information_From_GameStatus() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var player, displayName: "The OG", grandOverlordTime: TimeSpan.FromMinutes(1234))
                .WithGameStatus(1, grandOverlord: player, phase: GamePhase.Finished);

            var handler = new GetGameStatusQueryHandler(builder.Context);
            var query = new GetGameStatusQuery();

            var result = handler.Execute(query);

            result.CurrentGrandOverlordId.Should().Be(1);
            result.CurrentGrandOverlord.Should().Be("The OG");
            result.CurrentGrandOverlordTime.Should().Be(TimeSpan.FromMinutes(1234));
            result.Phase.Should().Be("Finished");
        }
    }
}
