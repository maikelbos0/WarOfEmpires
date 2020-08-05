using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.QueryHandlers.Attacks;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Tests.Attacks {
    [TestClass]
    public sealed class GetExecutedAttacksQueryHandlerTests {
        [TestMethod]
        public void GetExecutedAttacksQueryHandler_Returns_All_Executed_Attacks() {
            var builder = new FakeBuilder()
                .WithPlayer(2, out var defender1)
                .WithPlayer(3, out var defender2)
                .BuildPlayer(1)
                .WithAttackOn(1, defender1, AttackType.Raid, AttackResult.Won)
                .WithAttackOn(2, defender1, AttackType.Raid, AttackResult.Won)
                .WithAttackOn(3, defender2, AttackType.Raid, AttackResult.Won);

            var handler = new GetExecutedAttacksQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetExecutedAttacksQuery("test1@test.com");

            var result = handler.Execute(query);

            result.Should().HaveCount(3);
        }

        [TestMethod]
        public void GetExecutedAttacksQueryHandler_Returns_Correct_Data() {
            var builder = new FakeBuilder()
                .BuildAlliance(1, code: "DEF")
                .WithMember(2, out var defender, displayName: "Defender 1")
                .BuildPlayer(1)
                .BuildAttackOn(1, defender, AttackType.Raid, AttackResult.Won, turns: 7)
                .WithRound(false, new Casualties(TroopType.Archers, 0, 10), new Casualties(TroopType.Footmen, 0, 9), new Casualties(TroopType.Cavalry, 0, 8))
                .WithRound(false, new Casualties(TroopType.Archers, 4, 7), new Casualties(TroopType.Footmen, 3, 6), new Casualties(TroopType.Cavalry, 2, 5))
                .WithRound(true, new Casualties(TroopType.Archers, 0, 15), new Casualties(TroopType.Footmen, 0, 7), new Casualties(TroopType.Cavalry, 0, 3))
                .WithRound(true, new Casualties(TroopType.Archers, 3, 20), new Casualties(TroopType.Footmen, 2, 5), new Casualties(TroopType.Cavalry, 1, 3));

            var handler = new GetExecutedAttacksQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetExecutedAttacksQuery("test1@test.com");

            var result = handler.Execute(query);

            result.Should().HaveCount(1);
            result.Single().Id.Should().Be(1);
            result.Single().Date.Should().Be(new DateTime(2019, 1, 1));
            result.Single().Turns.Should().Be(7);
            result.Single().Type.Should().Be("Raid");
            result.Single().Defender.Should().Be("Defender 1");
            result.Single().DefenderAlliance.Should().Be("DEF");
            result.Single().DefenderSoldierCasualties.Should().Be(6);
            result.Single().DefenderMercenaryCasualties.Should().Be(53);
            result.Single().AttackerSoldierCasualties.Should().Be(9);
            result.Single().AttackerMercenaryCasualties.Should().Be(45);
            result.Single().Result.Should().Be("Won");
        }
    }
}