﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Attacks {
    [TestClass]
    public sealed class AssaultTests {
        [TestMethod]
        public void Assault_Result_Is_Surrendered_For_Defender_Low_Stamina() {
            var attacker = new Player(1, "Attacker", Race.Elves);
            var defender = new Player(2, "Defender", Race.Elves);

            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));
            defender.Troops.Add(new Troops(TroopType.Archers, 600, 200));

            typeof(Player).GetProperty(nameof(Player.Stamina)).SetValue(defender, 29);

            var attack = new Assault(attacker, defender, 10);
            attack.Execute();

            attack.Result.Should().Be(AttackResult.Surrendered);
        }

        [TestMethod]
        public void Assault_Result_Is_Surrendered_For_Defender_Without_Troops() {
            var attacker = new Player(1, "Attacker", Race.Elves);
            var defender = new Player(2, "Defender", Race.Elves);

            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));
            defender.Troops.Add(new Troops(TroopType.Archers, 0, 0));

            var attack = new Assault(attacker, defender, 10);
            attack.Execute();

            attack.Result.Should().Be(AttackResult.Surrendered);
        }

        [TestMethod]
        public void Assault_Surrendered_Calculates_Correct_Resources() {
            var attacker = new Player(1, "Attacker", Race.Elves);
            var defender = new Player(2, "Defender", Race.Elves);

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));
            defender.Troops.Add(new Troops(TroopType.Archers, 600, 200));

            typeof(Player).GetProperty(nameof(Player.Stamina)).SetValue(defender, 29);

            var expectedResources = new Resources(defender.Resources.Gold) * 0.25m;

            var attack = new Assault(attacker, defender, 10);
            attack.Execute();

            attack.Resources.Should().Be(expectedResources);
        }

        [TestMethod]
        public void Assault_Won_Calculates_Correct_Resources() {
            var attacker = new Player(1, "Attacker", Race.Elves);
            var defender = new Player(2, "Defender", Race.Elves);

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));
            defender.Troops.Add(new Troops(TroopType.Archers, 400, 100));

            var expectedResources = new Resources(defender.Resources.Gold) * 0.5m * (500m / 800m);

            var attack = new Assault(attacker, defender, 10);
            attack.Execute();

            attack.Resources.Should().Be(expectedResources);
        }

        [DataTestMethod]
        [DataRow(65, false, 0, 5, 1000, 3250, DisplayName = "Defender 5 turns")]
        [DataRow(65, true, 0, 5, 1000, 3250, DisplayName = "Attacker 5 turns")]
        [DataRow(95, false, 5, 10, 2000, 19000, DisplayName = "Defender 10 turns with defences")]
        [DataRow(95, true, 5, 10, 2000, 9500, DisplayName = "Attacker 10 turns with defences")]
        public void Assault_CalculateDamage_Is_Correct(int stamina, bool isAggressor, int defenceLevel, int turns, int troopAttackDamage, int expectedDamage) {
            var attacker = new Player(1, "Attacker", Race.Elves);
            var defender = new Player(2, "Defender", Race.Elves);
            var attack = new Assault(attacker, defender, turns);
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

        [DataTestMethod]
        [DataRow(29, 0, true, DisplayName = "Surrender without defences")]
        [DataRow(30, 0, false, DisplayName = "Not surrender without defences")]
        [DataRow(19, 5, true, DisplayName = "Surrender defences level 5")]
        [DataRow(20, 5, false, DisplayName = "Not surrender defences level 5")]
        [DataRow(14, 10, true, DisplayName = "Surrender defences level 10")]
        [DataRow(15, 10, false, DisplayName = "Not surrender defences level 10")]
        [DataRow(11, 15, true, DisplayName = "Surrender defences level 15")]
        [DataRow(12, 15, false, DisplayName = "Not surrender defences level 15")]
        public void Assault_IsSurrender_Is_Correct(int stamina, int defenceLevel, bool expectedResult) {
            var attacker = new Player(1, "Attacker", Race.Elves);
            var defender = new Player(2, "Defender", Race.Elves);
            var attack = new Assault(attacker, defender, 10);

            defender.Buildings.Add(new Building(BuildingType.Defences, defenceLevel));

            typeof(Player).GetProperty(nameof(Player.Stamina)).SetValue(defender, stamina);
            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));
            defender.Troops.Add(new Troops(TroopType.Archers, 600, 200));

            attack.IsSurrender().Should().Be(expectedResult);
        }

        [TestMethod]
        public void Assault_IsSurrender_Is_Affected_By_Troop_Size() {
            var attacker = new Player(1, "Attacker", Race.Elves);
            var defender = new Player(2, "Defender", Race.Elves);
            var attack = new Assault(attacker, defender, 10);

            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));
            defender.Troops.Add(new Troops(TroopType.Archers, 150, 25));

            attack.IsSurrender().Should().BeTrue();
        }
    }
}