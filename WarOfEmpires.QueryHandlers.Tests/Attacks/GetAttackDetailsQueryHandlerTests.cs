using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.QueryHandlers.Attacks;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Tests.Attacks {
    [TestClass]
    public sealed class GetAttackDetailsQueryHandlerTests {
        [TestMethod]
        public void GetAttackDetailsQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder().BuildAlliance(14, code: "DEF", name: "The Defenders")
                .BuildMember(1, email: "defender@test.com", displayName: "Defender 1")
                .GetPlayer(out var defender)
                .BuildAlliance(27, code: "ATK", name: "The Attackers")
                .BuildMember(2, email: "attacker@test.com", displayName: "Attacker 1")
                .BuildAttackOn(1, defender, AttackType.Raid, AttackResult.Won, resources: new Resources(1, 2, 3, 4, 5))
                .WithRound(true, 200, 17000, TroopType.Archers, new Casualties(TroopType.Archers, 0, 15), new Casualties(TroopType.Cavalry, 0, 14), new Casualties(TroopType.Footmen, 0, 13))
                .WithRound(false, 150, 16000, TroopType.Archers, new Casualties(TroopType.Archers, 0, 12), new Casualties(TroopType.Cavalry, 0, 11), new Casualties(TroopType.Footmen, 0, 10))
                .WithRound(false, 170, 15000, TroopType.Cavalry, new Casualties(TroopType.Archers, 1, 9), new Casualties(TroopType.Cavalry, 2, 8), new Casualties(TroopType.Footmen, 3, 7))
                .WithRound(true, 130, 14000, TroopType.Footmen, new Casualties(TroopType.Archers, 4, 6), new Casualties(TroopType.Cavalry, 5, 5), new Casualties(TroopType.Footmen, 6, 4));

            var handler = new GetAttackDetailsQueryHandler(builder.Context, new ResourcesMap(), new EnumFormatter());
            var query = new GetAttackDetailsQuery("defender@test.com", "1");

            var result = handler.Execute(query);

            result.Id.Should().Be(1);
            result.AttackerId.Should().Be(2);
            result.Attacker.Should().Be("Attacker 1");
            result.AttackerAllianceId.Should().Be(27);
            result.AttackerAllianceCode.Should().Be("ATK");
            result.AttackerAllianceName.Should().Be("The Attackers");
            result.DefenderId.Should().Be(1);
            result.Defender.Should().Be("Defender 1");
            result.DefenderAllianceId.Should().Be(14);
            result.DefenderAllianceCode.Should().Be("DEF");
            result.DefenderAllianceName.Should().Be("The Defenders");
            result.Date.Should().Be(new DateTime(2019, 1, 1));
            result.IsRead.Should().BeFalse();
            result.Turns.Should().Be(10);
            result.Type.Should().Be("Raid");
            result.Result.Should().Be("Won");
            result.Resources.Gold.Should().Be(1);
            result.Resources.Food.Should().Be(2);
            result.Resources.Wood.Should().Be(3);
            result.Resources.Stone.Should().Be(4);
            result.Resources.Ore.Should().Be(5);

            result.Rounds.Should().HaveCount(4);
            result.Rounds[2].IsAggressor.Should().BeFalse();
            result.Rounds[2].Attacker.Should().Be("Defender 1");
            result.Rounds[2].Defender.Should().Be("Attacker 1");
            result.Rounds[2].TroopType.Should().Be("Cavalry");
            result.Rounds[2].Troops.Should().Be(170);
            result.Rounds[2].Damage.Should().Be(15000);
            result.Rounds[2].SoldierCasualties.Should().Be(6);
            result.Rounds[2].MercenaryCasualties.Should().Be(24);
        }

        [TestMethod]
        public void GetAttackDetailsQueryHandler_Succeeds_For_Defender() {
            var builder = new FakeBuilder();
            var defender = builder.BuildAlliance(1)
                .BuildPlayer(1, email: "defender@test.com")
                .Player;

            builder.BuildAlliance(2)
                .BuildPlayer(2, email: "attacker@test.com")
                .BuildAttackOn(1, defender, AttackType.Raid, AttackResult.Won);

            var handler = new GetAttackDetailsQueryHandler(builder.Context, new ResourcesMap(), new EnumFormatter());
            var query = new GetAttackDetailsQuery("defender@test.com", "1");

            var result = handler.Execute(query);

            result.Should().NotBeNull();
        }

        [TestMethod]
        public void GetAttackDetailsQueryHandler_Succeeds_For_Attacker() {
            var builder = new FakeBuilder();
            var defender = builder.BuildAlliance(1)
                .BuildPlayer(1, email: "defender@test.com")
                .Player;

            builder.BuildAlliance(2)
                .BuildPlayer(2, email: "attacker@test.com")
                .BuildAttackOn(1, defender, AttackType.Raid, AttackResult.Won);

            var handler = new GetAttackDetailsQueryHandler(builder.Context, new ResourcesMap(), new EnumFormatter());
            var query = new GetAttackDetailsQuery("attacker@test.com", "1");

            var result = handler.Execute(query);

            result.Should().NotBeNull();
        }

        [TestMethod]
        public void GetAttackDetailsQueryHandler_IsRead_Is_Always_True_For_Attacker() {
            var builder = new FakeBuilder();
            var defender = builder.BuildAlliance(1)
                .BuildPlayer(1, email: "defender@test.com")
                .Player;

            builder.BuildAlliance(2)
                .BuildPlayer(2, email: "attacker@test.com")
                .BuildAttackOn(1, defender, AttackType.Raid, AttackResult.Won);

            var handler = new GetAttackDetailsQueryHandler(builder.Context, new ResourcesMap(), new EnumFormatter());
            var query = new GetAttackDetailsQuery("attacker@test.com", "1");

            var result = handler.Execute(query);

            result.IsRead.Should().BeTrue();
        }

        [TestMethod]
        public void GetAttackDetailsQueryHandler_Throws_Exception_For_Alphanumeric_AttackId() {
            var builder = new FakeBuilder();
            var defender = builder.BuildAlliance(1)
                .BuildPlayer(1, email: "defender@test.com")
                .Player;

            builder.BuildAlliance(2)
                .BuildPlayer(2, email: "attacker@test.com")
                .BuildAttackOn(1, defender, AttackType.Raid, AttackResult.Won);

            var handler = new GetAttackDetailsQueryHandler(builder.Context, new ResourcesMap(), new EnumFormatter());
            var query = new GetAttackDetailsQuery("attacker@test.com", "A");

            Action action = () => handler.Execute(query);

            action.Should().Throw<FormatException>();
        }

        [TestMethod]
        public void GetAttackDetailsQueryHandler_Throws_Exception_For_Attack_On_And_By_Different_Player() {
            var builder = new FakeBuilder();
            var defender = builder.BuildAlliance(1)
                .BuildPlayer(1, email: "defender@test.com")
                .Player;

            builder.BuildAlliance(2)
                .BuildPlayer(2, email: "attacker@test.com")
                .BuildAttackOn(1, defender, AttackType.Raid, AttackResult.Won);
            builder.BuildPlayer(3, email: "random@test.com");

            var handler = new GetAttackDetailsQueryHandler(builder.Context, new ResourcesMap(), new EnumFormatter());
            var query = new GetAttackDetailsQuery("random@test.com", "1");

            Action action = () => handler.Execute(query);

            action.Should().Throw<InvalidOperationException>();
        }
    }
}