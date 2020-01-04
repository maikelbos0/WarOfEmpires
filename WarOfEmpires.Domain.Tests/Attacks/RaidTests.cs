using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Attacks {
    [TestClass]
    public sealed class RaidTests {
        [TestMethod]
        public void Raid_Result_Is_Surrendered_For_Defender_Low_Stamina() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            attacker.TrainTroops(600, 200, 0, 0, 0, 0);
            defender.TrainTroops(600, 200, 0, 0, 0, 0);

            typeof(Player).GetProperty(nameof(Player.Stamina)).SetValue(defender, 29);

            var attack = new Raid(attacker, defender, 10);
            attack.Execute();

            attack.Result.Should().Be(AttackResult.Surrendered);
        }

        [TestMethod]
        public void Raid_Surrendered_Gives_Correct_Resources_To_Attacker() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            attacker.TrainTroops(600, 200, 0, 0, 0, 0);
            defender.TrainTroops(600, 200, 0, 0, 0, 0);

            typeof(Player).GetProperty(nameof(Player.Stamina)).SetValue(defender, 29);

            var expectedResources = (defender.Resources - new Resources(defender.Resources.Gold)) * 0.25m;
            var previousDefenderResources = defender.Resources;
            var previousAttackerResources = attacker.Resources;

            var attack = new Raid(attacker, defender, 10);
            attack.Execute();

            attack.Resources.Should().Be(expectedResources);
            defender.Resources.Should().Be(previousDefenderResources - expectedResources);
            attacker.Resources.Should().Be(previousAttackerResources + expectedResources);
        }

        [TestMethod]
        public void Raid_Won_Gives_Correct_Resources_To_Attacker() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            attacker.TrainTroops(600, 200, 0, 0, 0, 0);
            defender.TrainTroops(400, 100, 0, 0, 0, 0);

            var expectedResources = (defender.Resources - new Resources(defender.Resources.Gold)) * 0.5m;
            var previousDefenderResources = defender.Resources;
            var previousAttackerResources = attacker.Resources;

            var attack = new Raid(attacker, defender, 10);
            attack.Execute();

            attack.Resources.Should().Be(expectedResources);
            defender.Resources.Should().Be(previousDefenderResources - expectedResources);
            attacker.Resources.Should().Be(previousAttackerResources + expectedResources);
        }

        [DataTestMethod]
        [DataRow(65, false, 5, 1000, 3250, DisplayName = "Defender 5 turns")]
        [DataRow(65, true, 5, 1000, 3250, DisplayName = "Attacker 5 turns")]
        [DataRow(95, false, 10, 2000, 19000, DisplayName = "Defender 10 turns")]
        [DataRow(95, true, 10, 2000, 19000, DisplayName = "Attacker 10 turns")]
        public void Raid_CalculateDamage_Is_Correct(int stamina, bool isAggressor, int turns, int troopAttackDamage, int expectedDamage) {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");
            var attack = new Raid(attacker, defender, turns);
            var troopInfo = Substitute.For<TroopInfo>();

            defender.Buildings.Add(new Building(defender, BuildingType.Defences, 5));
            troopInfo.GetTotalAttack().Returns(troopAttackDamage);

            attack.CalculateDamage(stamina, isAggressor, troopInfo, defender).Should().Be(expectedDamage);
        }

        [DataTestMethod]
        [DataRow(29, true, DisplayName ="Surrender")]
        [DataRow(30, false, DisplayName = "Not surrender")]
        public void Raid_IsSurrender_Is_Correct(int stamina, bool expectedResult) {
            var defender = Substitute.For<Player>();
            var attack = new Raid(null, defender, 10);

            defender.Stamina.Returns(stamina);

            attack.IsSurrender().Should().Be(expectedResult);
        }
    }
}