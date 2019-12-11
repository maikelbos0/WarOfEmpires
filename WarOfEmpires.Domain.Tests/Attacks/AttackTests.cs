using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Attacks {
    [TestClass]
    public sealed class AttackTests {
        [TestMethod]
        public void Attack_1() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            typeof(Player).GetProperty(nameof(Player.Peasants)).SetValue(attacker, 1000);
            typeof(Player).GetProperty(nameof(Player.Peasants)).SetValue(defender, 1000);

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

            results.Should().BeNull();
        }

        [TestMethod]
        public void Attack_2() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            typeof(Player).GetProperty(nameof(Player.Peasants)).SetValue(attacker, 1000);
            typeof(Player).GetProperty(nameof(Player.Peasants)).SetValue(defender, 1000);

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

            results.Should().BeNull();
        }

        [TestMethod]
        public void Attack_3() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            typeof(Player).GetProperty(nameof(Player.Peasants)).SetValue(attacker, 1000);
            typeof(Player).GetProperty(nameof(Player.Peasants)).SetValue(defender, 1000);

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

            results.Should().BeNull();
        }
    }
}