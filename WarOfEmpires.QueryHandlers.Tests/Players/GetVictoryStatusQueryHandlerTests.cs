using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Players {
    [TestClass]
    public sealed class GetVictoryStatusQueryHandlerTests {
        [TestMethod]
        public void GetVictoryStatusQueryHandler_Returns_No_GrandOverlord_By_Default() {
            var builder = new FakeBuilder()
                .WithPlayer(1);

            var handler = new GetVictoryStatusQueryHandler(builder.Context);
            var query = new GetVictoryStatusQuery();

            var result = handler.Execute(query);

            result.CurrentGrandOverlordId.Should().BeNull();
            result.CurrentGrandOverlord.Should().BeNull();
            result.GrandOverlordTime.Should().BeNull();
        }

        [TestMethod]
        public void GetVictoryStatusQueryHandler_Returns_GrandOverlord_If_Available() {
            var builder = new FakeBuilder()
                .BuildPlayer(1, displayName: "Winner", title: TitleType.GrandOverlord, grandOverlordTime: TimeSpan.FromMinutes(1234));

            var handler = new GetVictoryStatusQueryHandler(builder.Context);
            var query = new GetVictoryStatusQuery();

            var result = handler.Execute(query);

            result.CurrentGrandOverlordId.Should().Be(1);
            result.CurrentGrandOverlord.Should().Be("Winner");
            result.GrandOverlordTime.Should().Be(TimeSpan.FromMinutes(1234));
        }

        [TestMethod]
        public void GetVictoryStatusQueryHandler_Returns_No_GrandOverlord_If_Inactive() {
            var builder = new FakeBuilder()
                .WithPlayer(1, title: TitleType.GrandOverlord, status: UserStatus.Inactive);

            var handler = new GetVictoryStatusQueryHandler(builder.Context);
            var query = new GetVictoryStatusQuery();

            var result = handler.Execute(query);

            result.CurrentGrandOverlordId.Should().BeNull();
            result.CurrentGrandOverlord.Should().BeNull();
            result.GrandOverlordTime.Should().BeNull();
        }
    }
}
