using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Queries.Game;
using WarOfEmpires.QueryHandlers.Players;
using WarOfEmpires.Utilities.Game;

namespace WarOfEmpires.QueryHandlers.Tests.Game {
    [TestClass]
    public sealed class GetGameStatusQueryHandlerTests {
        [TestMethod]
        public void GetGameStatusQueryHandler_Returns_Information_From_GameStatus() {
            var gameStatus = new GameStatus() { CurrentGrandOverlordId = 1, CurrentGrandOverlord = "The OG", CurrentGrandOverlordTime = TimeSpan.FromMinutes(1234) };
            var handler = new GetGameStatusQueryHandler(gameStatus);
            var query = new GetGameStatusQuery();

            var result = handler.Execute(query);

            result.CurrentGrandOverlordId.Should().Be(1);
            result.CurrentGrandOverlord.Should().Be("The OG");
            result.CurrentGrandOverlordTime.Should().Be(TimeSpan.FromMinutes(1234));
        }
    }
}
