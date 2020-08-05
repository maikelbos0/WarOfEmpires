using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Linq;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.QueryHandlers.Empires;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Tests.Empires {
    [TestClass]
    public sealed class GetTroopsQueryHandlerTests {
        [TestMethod]
        public void GetTroopsQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithTroops(TroopType.Archers, 7, 6)
                .WithTroops(TroopType.Cavalry, 5, 4)
                .WithTroops(TroopType.Footmen, 3, 2)
                .WithPeasants(1);

            builder.Player.WillUpkeepRunOut().Returns(true);
            builder.Player.GetSoldierRecruitsPenalty().Returns(1);
            builder.Player.HasUpkeepRunOut.Returns(true);
            builder.Player.Stamina.Returns(100);

            var query = new GetTroopsQuery("test1@test.com");
            var handler = new GetTroopsQueryHandler(builder.Context, new ResourcesMap(), new EnumFormatter());

            var result = handler.Execute(query);

            result.CurrentPeasants.Should().Be(1);
            result.MercenaryTrainingCost.Gold.Should().Be(5000);
            result.WillUpkeepRunOut.Should().BeTrue();
            result.HasUpkeepRunOut.Should().BeTrue();
            result.HasSoldierShortage.Should().BeTrue();
            result.CurrentStamina.Should().Be(100);
            result.StaminaToHeal.Should().Be("0");
            result.Troops.Should().HaveCount(3);

            var troop = result.Troops.Single(t => t.Type == "Archers");

            troop.Name.Should().Be("Archers");
            troop.CurrentSoldiers.Should().Be(7);
            troop.CurrentMercenaries.Should().Be(6);
            troop.Cost.Gold.Should().Be(5000);
            troop.Cost.Wood.Should().Be(1000);
            troop.Cost.Ore.Should().Be(500);
        }
    }
}