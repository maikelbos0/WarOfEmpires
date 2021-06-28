using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Domain.Game;
using WarOfEmpires.Queries.Game;
using WarOfEmpires.QueryHandlers.Game;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Game {
    [TestClass]
    public sealed class GetGamePhaseQueryHandlerTests {
        [TestMethod]
        public void GetGamePhaseQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .WithGameStatus(1, phase: GamePhase.Finished);

            var handler = new GetGamePhaseQueryHandler(builder.Context);
            var query = new GetGamePhaseQuery();

            var result = handler.Execute(query);

            result.Phase.Should().Be("Finished");
        }
    }
}
