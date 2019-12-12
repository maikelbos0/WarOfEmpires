using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Attacks {
    [TestClass]
    public sealed class AttackTests {
        [TestMethod]
        public void Attack_Result_Is_Surrender_For_Defender_Low_Stamina() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            
            attacker.TrainTroops(600, 200, 0, 0, 0, 0);
            defender.TrainTroops(600, 200, 0, 0, 0, 0);

            typeof(Player).GetProperty(nameof(Player.Stamina)).SetValue(defender, 29);

            var attack = new Attack(attacker, defender, 10);
            attack.Execute();

            attack.Result.Should().Be(AttackResult.Surrendered);
        }

        [TestMethod]
        public void Attack_Result_Is_Fatigued_For_Attacker_Low_Stamina() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            attacker.TrainTroops(600, 200, 0, 0, 0, 0);
            defender.TrainTroops(600, 200, 0, 0, 0, 0);

            typeof(Player).GetProperty(nameof(Player.Stamina)).SetValue(attacker, 69);

            var attack = new Attack(attacker, defender, 10);
            attack.Execute();

            attack.Result.Should().Be(AttackResult.Fatigued);
        }

        public void Attack_Result_Is_Won_For_Stronger_Attacker() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            attacker.TrainTroops(600, 200, 0, 0, 0, 0);
            defender.TrainTroops(400, 100, 0, 0, 0, 0);

            var attack = new Attack(attacker, defender, 10);
            attack.Execute();

            attack.Result.Should().Be(AttackResult.Won);
        }

        public void Attack_Result_Is_Defended_For_Stronger_Defender() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            attacker.TrainTroops(400, 100, 0, 0, 0, 0);
            defender.TrainTroops(600, 200, 0, 0, 0, 0);

            var attack = new Attack(attacker, defender, 10);
            attack.Execute();

            attack.Result.Should().Be(AttackResult.Defended);
        }

        [DataTestMethod]
        [DataRow(-1, DisplayName = "Negative")]
        [DataRow(0, DisplayName = "Zero")]
        [DataRow(11, DisplayName = "Too many")]
        public void Attack_Constructor_Throws_Exception_For_Invalid_Turns(int turns) {
            Action action = () => new Attack(null, null, turns);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Attack_Creates_Correct_Rounds_One_TroopType() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            attacker.TrainTroops(600, 200, 0, 0, 0, 0);
            defender.TrainTroops(600, 200, 0, 0, 0, 0);

            var attack = new Attack(attacker, defender, 10);
            attack.Execute();

            var rounds = attack.Rounds.ToList();
            rounds.Should().HaveCount(2);
            rounds[0].TroopType.Should().Be(TroopType.Archers);
            rounds[0].IsAggressor.Should().BeTrue();
            rounds[1].TroopType.Should().Be(TroopType.Archers);
            rounds[1].IsAggressor.Should().BeFalse();
        }

        [TestMethod]
        public void Attack_Creates_Correct_Rounds_One_Vs_Three_TroopTypes() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            attacker.TrainTroops(600, 200, 0, 0, 0, 0);
            defender.TrainTroops(200, 50, 200, 50, 200, 50);

            var attack = new Attack(attacker, defender, 10);
            attack.Execute();

            var rounds = attack.Rounds.ToList();
            rounds.Should().HaveCount(4);
            rounds[0].TroopType.Should().Be(TroopType.Archers);
            rounds[0].IsAggressor.Should().BeTrue();
            rounds[1].TroopType.Should().Be(TroopType.Archers);
            rounds[1].IsAggressor.Should().BeFalse();
            rounds[2].TroopType.Should().Be(TroopType.Cavalry);
            rounds[2].IsAggressor.Should().BeFalse();
            rounds[3].TroopType.Should().Be(TroopType.Footmen);
            rounds[3].IsAggressor.Should().BeFalse();
        }

        [TestMethod]
        public void Attack_Creates_Correct_Rounds_Two_TroopTypes() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            attacker.TrainTroops(300, 100, 300, 100, 0, 0);
            defender.TrainTroops(0, 0, 300, 100, 300, 100);

            var attack = new Attack(attacker, defender, 10);
            attack.Execute();

            var rounds = attack.Rounds.ToList();
            rounds.Should().HaveCount(4);
            rounds[0].TroopType.Should().Be(TroopType.Archers);
            rounds[0].IsAggressor.Should().BeTrue();
            rounds[1].TroopType.Should().Be(TroopType.Cavalry);
            rounds[1].IsAggressor.Should().BeTrue();
            rounds[2].TroopType.Should().Be(TroopType.Cavalry);
            rounds[2].IsAggressor.Should().BeFalse();
            rounds[3].TroopType.Should().Be(TroopType.Footmen);
            rounds[3].IsAggressor.Should().BeFalse();
        }

        [TestMethod]
        public void Attack_Creates_Correct_Rounds_Three_TroopTypes() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            attacker.TrainTroops(200, 50, 200, 50, 200, 50);
            defender.TrainTroops(200, 50, 200, 50, 200, 50);

            var attack = new Attack(attacker, defender, 10);
            attack.Execute();

            var rounds = attack.Rounds.ToList();
            rounds.Should().HaveCount(6);
            rounds[0].TroopType.Should().Be(TroopType.Archers);
            rounds[0].IsAggressor.Should().BeTrue();
            rounds[1].TroopType.Should().Be(TroopType.Archers);
            rounds[1].IsAggressor.Should().BeFalse();
            rounds[2].TroopType.Should().Be(TroopType.Cavalry);
            rounds[2].IsAggressor.Should().BeTrue();
            rounds[3].TroopType.Should().Be(TroopType.Cavalry);
            rounds[3].IsAggressor.Should().BeFalse();
            rounds[4].TroopType.Should().Be(TroopType.Footmen);
            rounds[4].IsAggressor.Should().BeTrue();
            rounds[5].TroopType.Should().Be(TroopType.Footmen);
            rounds[5].IsAggressor.Should().BeFalse();
        }

        [TestMethod]
        [Ignore]
        public void Attack_1() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            attacker.TrainTroops(600, 200, 0, 0, 0, 0);
            defender.TrainTroops(600, 200, 0, 0, 0, 0);

            var attack = new Attack(attacker, defender, 10);
            attack.Execute();

            var results = new {
                attack.Result,
                AttackerStamina = attacker.Stamina,
                DefenderStamina = defender.Stamina,
                AttackerSoldierCasualties = attack.Rounds.Where(r => !r.IsAggressor).Sum(r => r.Casualties.Archers.Soldiers + r.Casualties.Cavalry.Soldiers + r.Casualties.Footmen.Soldiers),
                DefenderSoldierCasualties = attack.Rounds.Where(r => r.IsAggressor).Sum(r => r.Casualties.Archers.Soldiers + r.Casualties.Cavalry.Soldiers + r.Casualties.Footmen.Soldiers),
                AttackerMercenaryCasualties = attack.Rounds.Where(r => !r.IsAggressor).Sum(r => r.Casualties.Archers.Mercenaries + r.Casualties.Cavalry.Mercenaries + r.Casualties.Footmen.Mercenaries),
                DefenderMercenaryCasualties = attack.Rounds.Where(r => r.IsAggressor).Sum(r => r.Casualties.Archers.Mercenaries + r.Casualties.Cavalry.Mercenaries + r.Casualties.Footmen.Mercenaries)
            };
        }

        [TestMethod]
        [Ignore]
        public void Attack_2() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            attacker.TrainTroops(0, 0, 600, 200, 0, 0);
            defender.TrainTroops(0, 0, 600, 200, 0, 0);

            var attack = new Attack(attacker, defender, 10);
            attack.Execute();

            var results = new {
                attack.Result,
                AttackerStamina = attacker.Stamina,
                DefenderStamina = defender.Stamina,
                AttackerSoldierCasualties = attack.Rounds.Where(r => !r.IsAggressor).Sum(r => r.Casualties.Archers.Soldiers + r.Casualties.Cavalry.Soldiers + r.Casualties.Footmen.Soldiers),
                DefenderSoldierCasualties = attack.Rounds.Where(r => r.IsAggressor).Sum(r => r.Casualties.Archers.Soldiers + r.Casualties.Cavalry.Soldiers + r.Casualties.Footmen.Soldiers),
                AttackerMercenaryCasualties = attack.Rounds.Where(r => !r.IsAggressor).Sum(r => r.Casualties.Archers.Mercenaries + r.Casualties.Cavalry.Mercenaries + r.Casualties.Footmen.Mercenaries),
                DefenderMercenaryCasualties = attack.Rounds.Where(r => r.IsAggressor).Sum(r => r.Casualties.Archers.Mercenaries + r.Casualties.Cavalry.Mercenaries + r.Casualties.Footmen.Mercenaries)
            };
        }

        [TestMethod]
        [Ignore]
        public void Attack_3() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            attacker.TrainTroops(0, 0, 0, 0, 600, 200);
            defender.TrainTroops(0, 0, 0, 0, 600, 200);

            var attack = new Attack(attacker, defender, 10);
            attack.Execute();

            var results = new {
                attack.Result,
                AttackerStamina = attacker.Stamina,
                DefenderStamina = defender.Stamina,
                AttackerSoldierCasualties = attack.Rounds.Where(r => !r.IsAggressor).Sum(r => r.Casualties.Archers.Soldiers + r.Casualties.Cavalry.Soldiers + r.Casualties.Footmen.Soldiers),
                DefenderSoldierCasualties = attack.Rounds.Where(r => r.IsAggressor).Sum(r => r.Casualties.Archers.Soldiers + r.Casualties.Cavalry.Soldiers + r.Casualties.Footmen.Soldiers),
                AttackerMercenaryCasualties = attack.Rounds.Where(r => !r.IsAggressor).Sum(r => r.Casualties.Archers.Mercenaries + r.Casualties.Cavalry.Mercenaries + r.Casualties.Footmen.Mercenaries),
                DefenderMercenaryCasualties = attack.Rounds.Where(r => r.IsAggressor).Sum(r => r.Casualties.Archers.Mercenaries + r.Casualties.Cavalry.Mercenaries + r.Casualties.Footmen.Mercenaries)
            };
        }
    }
}