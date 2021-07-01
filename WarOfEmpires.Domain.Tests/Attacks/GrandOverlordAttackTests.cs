using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Attacks {
    [TestClass]
    public sealed class GrandOverlordAttackTests {
        [TestMethod]
        public void GrandOverlordAttack_Result_Is_Never_Surrendered() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));
            defender.Troops.Add(new Troops(TroopType.Archers, 10, 2));
            defender.Buildings.Add(new Building(BuildingType.Defences, 1));

            typeof(Player).GetProperty(nameof(Player.Stamina)).SetValue(defender, 1);

            var attack = new GrandOverlordAttack(attacker, defender, 10);
            attack.Execute();

            attack.Result.Should().Be(AttackResult.Won);
        }

        [TestMethod]
        public void GrandOverlordAttack_Result_Is_Surrendered_For_Defender_Without_Troops() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));
            defender.Troops.Add(new Troops(TroopType.Archers, 0, 0));

            var attack = new GrandOverlordAttack(attacker, defender, 10);
            attack.Execute();

            attack.Result.Should().Be(AttackResult.Surrendered);
        }

        [TestMethod]
        public void GrandOverlordAttack_Won_Calculates_Correct_Resources() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));
            defender.Troops.Add(new Troops(TroopType.Archers, 400, 100));

            var attack = new GrandOverlordAttack(attacker, defender, 10);
            attack.Execute();

            attack.Resources.Should().Be(new Resources());
        }

        [DataTestMethod]
        [DataRow(65, false, 0, 5, 1000, 3250, DisplayName = "Defender 5 turns")]
        [DataRow(65, true, 0, 5, 1000, 3250, DisplayName = "Attacker 5 turns")]
        [DataRow(95, false, 5, 10, 2000, 19000, DisplayName = "Defender 10 turns with defences")]
        [DataRow(95, true, 5, 10, 2000, 9500, DisplayName = "Attacker 10 turns with defences")]
        public void GrandOverlordAttack_CalculateDamage_Is_Correct(int stamina, bool isAggressor, int defenceLevel, int turns, int troopAttackDamage, int expectedDamage) {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");
            var attack = new GrandOverlordAttack(attacker, defender, turns);
            var troopInfo = Substitute.For<TroopInfo>();

            defender.Buildings.Add(new Building(BuildingType.Defences, defenceLevel));
            if (isAggressor) {
                troopInfo.GetTotalAttack(Arg.Any<double>()).Returns(c => (long)(c.Arg<double>() * troopAttackDamage));
            }
            else {
                troopInfo.GetTotalAttack(Arg.Any<double>()).Returns(troopAttackDamage);
            }            

            attack.CalculateDamage(stamina, isAggressor, troopInfo, defender).Should().Be(expectedDamage);
        }
    }
}