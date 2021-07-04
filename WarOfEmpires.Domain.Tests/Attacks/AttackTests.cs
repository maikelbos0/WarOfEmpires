using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Attacks {
    [TestClass]
    public sealed class AttackTests {
        // All tests here apply to all attack types

        [TestMethod]
        public void Attack_IsAtWar_Is_Correct_For_No_Alliance() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));

            var attack = new Assault(attacker, defender, 10);

            attack.IsAtWar.Should().BeFalse();
        }

        [TestMethod]
        public void Attack_IsAtWar_Is_Correct_For_Alliance_Without_War() {            
            var attacker = new Player(1, "Attacker");            
            var defender = new Player(2, "Defender");
            var attackerAlliance = new Alliance(attacker, "ATK", "The Attackers");
            var defenderAlliance = new Alliance(defender, "DEF", "The Defenders");

            typeof(Player).GetProperty(nameof(Player.Alliance)).SetValue(attacker, attackerAlliance);
            typeof(Player).GetProperty(nameof(Player.Alliance)).SetValue(defender, defenderAlliance);
            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));

            var attack = new Assault(attacker, defender, 10);

            attack.IsAtWar.Should().BeFalse();
        }

        [TestMethod]
        public void Attack_IsAtWar_Is_Correct_For_Alliance_With_War() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");
            var attackerAlliance = new Alliance(attacker, "ATK", "The Attackers");
            var defenderAlliance = new Alliance(defender, "DEF", "The Defenders");

            typeof(Player).GetProperty(nameof(Player.Alliance)).SetValue(attacker, attackerAlliance);
            typeof(Player).GetProperty(nameof(Player.Alliance)).SetValue(defender, defenderAlliance);
            attackerAlliance.DeclareWar(defenderAlliance);
            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));

            var attack = new Assault(attacker, defender, 10);

            attack.IsAtWar.Should().BeTrue();
        }

        [TestMethod]
        public void Attack_HasWarDamage_Is_Correct_For_No_Alliance() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));

            var attack = new Assault(attacker, defender, 10);

            attack.HasWarDamage.Should().BeFalse();
        }

        [TestMethod]
        public void Attack_HasWarDamage_Is_Correct_For_Alliance_Without_War() {            
            var attacker = new Player(1, "Attacker");            
            var defender = new Player(2, "Defender");
            var attackerAlliance = new Alliance(attacker, "ATK", "The Attackers");
            var defenderAlliance = new Alliance(defender, "DEF", "The Defenders");

            typeof(Player).GetProperty(nameof(Player.Alliance)).SetValue(attacker, attackerAlliance);
            typeof(Player).GetProperty(nameof(Player.Alliance)).SetValue(defender, defenderAlliance);
            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));

            var attack = new Assault(attacker, defender, 10);

            attack.HasWarDamage.Should().BeFalse();
        }

        [TestMethod]
        public void Attack_HasWarDamage_Is_Correct_For_Alliance_With_War() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");
            var attackerAlliance = new Alliance(attacker, "ATK", "The Attackers");
            var defenderAlliance = new Alliance(defender, "DEF", "The Defenders");

            typeof(Player).GetProperty(nameof(Player.Alliance)).SetValue(attacker, attackerAlliance);
            typeof(Player).GetProperty(nameof(Player.Alliance)).SetValue(defender, defenderAlliance);
            attackerAlliance.DeclareWar(defenderAlliance);
            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));

            var attack = new Assault(attacker, defender, 10);

            attack.HasWarDamage.Should().BeTrue();
        }
         
        [TestMethod]
        public void Attack_HasWarDamage_Is_Correct_When_Received_War_Attack_Recently() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");
            var receivedAttack = new Assault(defender, attacker, 10);

            typeof(Assault).GetProperty(nameof(Assault.IsAtWar)).SetValue(receivedAttack, true);
            typeof(Assault).GetProperty(nameof(Assault.Date)).SetValue(receivedAttack, DateTime.UtcNow.AddHours(-15).AddMinutes(-59));

            attacker.ReceivedAttacks.Add(receivedAttack);
            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));

            var attack = new Assault(attacker, defender, 10);

            attack.HasWarDamage.Should().BeTrue();
        }
         
        [TestMethod]
        public void Attack_HasWarDamage_Is_Correct_When_Received_War_Attack_Expired() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");
            var receivedAttack = new Assault(defender, attacker, 10);

            typeof(Assault).GetProperty(nameof(Assault.IsAtWar)).SetValue(receivedAttack, true);
            typeof(Assault).GetProperty(nameof(Assault.Date)).SetValue(receivedAttack, DateTime.UtcNow.AddHours(-16).AddMinutes(-1));

            defender.ReceivedAttacks.Add(receivedAttack);
            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));

            var attack = new Assault(attacker, defender, 10);

            attack.HasWarDamage.Should().BeFalse();
        }

        [TestMethod]
        public void Attack_Result_Is_Fatigued_for_Attacker_Without_Troops() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            defender.Troops.Add(new Troops(TroopType.Archers, 600, 200));

            var attack = new Raid(attacker, defender, 10);
            attack.Execute();

            attack.Result.Should().Be(AttackResult.Fatigued);
        }

        [TestMethod]
        public void Attack_Result_Is_Fatigued_For_Attacker_Low_Stamina() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));
            defender.Troops.Add(new Troops(TroopType.Archers, 600, 200));

            typeof(Player).GetProperty(nameof(Player.Stamina)).SetValue(attacker, 69);

            var attack = new Raid(attacker, defender, 10);
            attack.Execute();

            attack.Result.Should().Be(AttackResult.Fatigued);
        }

        public void Attack_Result_Is_Won_For_Stronger_Attacker() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));
            defender.Troops.Add(new Troops(TroopType.Archers, 400, 100));

            var attack = new Raid(attacker, defender, 10);
            attack.Execute();

            attack.Result.Should().Be(AttackResult.Won);
        }

        public void Attack_Result_Is_Defended_For_Stronger_Defender() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            attacker.Troops.Add(new Troops(TroopType.Archers, 400, 100));
            defender.Troops.Add(new Troops(TroopType.Archers, 600, 200));

            var attack = new Raid(attacker, defender, 10);
            attack.Execute();

            attack.Result.Should().Be(AttackResult.Defended);
        }

        [TestMethod]
        public void Attack_Surrendered_Calculates_Correct_Minimum_Resources() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));

            var expectedResources = new Resources(defender.Resources.Gold) * 0.25m * 0.5m;

            var attack = new Assault(attacker, defender, 10);
            attack.Execute();

            attack.Resources.Should().Be(expectedResources);
        }

        [TestMethod]
        public void Attack_Won_Calculates_Correct_Minimum_Resources() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Stamina)).SetValue(attacker, 70);

            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));
            defender.Troops.Add(new Troops(TroopType.Archers, 300, 75));

            var expectedResources = new Resources(defender.Resources.Gold) * 0.5m * 0.5m;

            var attack = new Assault(attacker, defender, 10);
            attack.Execute();

            attack.Resources.Should().Be(expectedResources);
        }

        [TestMethod]
        public void Attack_Surrendered_With_War_Damage_Calculates_Correct_Resources() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));
            defender.Troops.Add(new Troops(TroopType.Archers, 600, 200));

            typeof(Player).GetProperty(nameof(Player.Stamina)).SetValue(defender, 29);

            var expectedResources = new Resources(defender.Resources.Gold) * 0.25m * 2m;

            var attack = new Assault(attacker, defender, 10);
            typeof(Assault).GetProperty(nameof(Assault.HasWarDamage)).SetValue(attack, true);

            attack.Execute();

            attack.Resources.Should().Be(expectedResources);
        }

        [TestMethod]
        public void Attack_Won_With_War_Damage_Calculates_Correct_Resources() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));
            defender.Troops.Add(new Troops(TroopType.Archers, 400, 100));

            var expectedResources = new Resources(defender.Resources.Gold) * 0.5m * (500m / 800m) * 2m;

            var attack = new Assault(attacker, defender, 10);
            typeof(Assault).GetProperty(nameof(Assault.HasWarDamage)).SetValue(attack, true);

            attack.Execute();

            attack.Resources.Should().Be(expectedResources);
        }

        [TestMethod]
        public void Attack_Resources_Are_Never_More_Than_Defender_Resources() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));
            defender.Troops.Add(new Troops(TroopType.Archers, 700, 220));

            typeof(Player).GetProperty(nameof(Player.Stamina)).SetValue(defender, 50);

            var expectedResources = new Resources(gold: 10000000);

            var attack = new Assault(attacker, defender, 10);
            typeof(Assault).GetProperty(nameof(Assault.HasWarDamage)).SetValue(attack, true);

            attack.Execute();

            attack.Resources.Should().Be(expectedResources);
        }

        [TestMethod]
        public void Attack_Fatigued_Results_In_No_Resources() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));
            defender.Troops.Add(new Troops(TroopType.Archers, 600, 200));

            typeof(Player).GetProperty(nameof(Player.Stamina)).SetValue(attacker, 69);

            var attack = new Raid(attacker, defender, 10);
            attack.Execute();

            attack.Resources.Should().Be(new Resources());
        }

        [TestMethod]
        public void Attack_Defended_Results_In_No_Resources() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            attacker.Troops.Add(new Troops(TroopType.Archers, 400, 100));
            defender.Troops.Add(new Troops(TroopType.Archers, 600, 200));

            var attack = new Raid(attacker, defender, 10);
            attack.Execute();

            attack.Resources.Should().Be(new Resources());
        }

        [DataTestMethod]
        [DataRow(-1, DisplayName = "Negative")]
        [DataRow(0, DisplayName = "Zero")]
        [DataRow(11, DisplayName = "Too many")]
        public void Attack_Constructor_Throws_Exception_For_Invalid_Turns(int turns) {
            Action action = () => new Raid(null, null, turns);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Attack_Creates_Correct_Rounds_One_TroopType() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));
            defender.Troops.Add(new Troops(TroopType.Archers, 600, 200));

            var attack = new Raid(attacker, defender, 10);
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

            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));
            defender.Troops.Add(new Troops(TroopType.Archers, 200, 50));
            defender.Troops.Add(new Troops(TroopType.Cavalry, 200, 50));
            defender.Troops.Add(new Troops(TroopType.Footmen, 200, 50));

            var attack = new Raid(attacker, defender, 10);
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

            attacker.Troops.Add(new Troops(TroopType.Archers, 300, 100));
            attacker.Troops.Add(new Troops(TroopType.Cavalry, 300, 100));
            defender.Troops.Add(new Troops(TroopType.Cavalry, 300, 100));
            defender.Troops.Add(new Troops(TroopType.Footmen, 300, 100));

            var attack = new Raid(attacker, defender, 10);
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

            attacker.Troops.Add(new Troops(TroopType.Archers, 200, 50));
            attacker.Troops.Add(new Troops(TroopType.Cavalry, 200, 50));
            attacker.Troops.Add(new Troops(TroopType.Footmen, 200, 50));
            defender.Troops.Add(new Troops(TroopType.Archers, 200, 50));
            defender.Troops.Add(new Troops(TroopType.Cavalry, 200, 50));
            defender.Troops.Add(new Troops(TroopType.Footmen, 200, 50));

            var attack = new Raid(attacker, defender, 10);
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
        public void Attack_GetArmyStrengthModifier_Minimum_Works() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 130));
            attacker.Troops.Add(new Troops(TroopType.Cavalry, 10, 15));
            attacker.Troops.Add(new Troops(TroopType.Footmen, 20, 25));

            var attack = new Raid(attacker, defender, 10);

            attack.GetArmyStrengthModifier(0.5m).Should().Be(0.5m);
        }

        [TestMethod]
        public void Attack_GetArmyStrengthModifier_Works_Small_Defender() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 130));
            attacker.Troops.Add(new Troops(TroopType.Cavalry, 10, 15));
            attacker.Troops.Add(new Troops(TroopType.Footmen, 20, 25));
            defender.Troops.Add(new Troops(TroopType.Archers, 400, 130));
            defender.Troops.Add(new Troops(TroopType.Cavalry, 10, 15));
            defender.Troops.Add(new Troops(TroopType.Footmen, 20, 25));

            var attack = new Raid(attacker, defender, 10);

            attack.GetArmyStrengthModifier().Should().Be(0.75m);
        }

        [TestMethod]
        public void Attack_GetArmyStrengthModifier_Works_Medium_Defender() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 130));
            attacker.Troops.Add(new Troops(TroopType.Cavalry, 10, 15));
            attacker.Troops.Add(new Troops(TroopType.Footmen, 20, 25));
            defender.Troops.Add(new Troops(TroopType.Archers, 600, 130));
            defender.Troops.Add(new Troops(TroopType.Cavalry, 10, 15));
            defender.Troops.Add(new Troops(TroopType.Footmen, 20, 25));

            var attack = new Raid(attacker, defender, 10);

            attack.GetArmyStrengthModifier().Should().Be(1.0m);
        }

        [TestMethod]
        public void Attack_GetArmyStrengthModifier_Works_Big_Defender() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 130));
            attacker.Troops.Add(new Troops(TroopType.Cavalry, 10, 15));
            attacker.Troops.Add(new Troops(TroopType.Footmen, 20, 25));
            defender.Troops.Add(new Troops(TroopType.Archers, 760, 130));
            defender.Troops.Add(new Troops(TroopType.Cavalry, 10, 15));
            defender.Troops.Add(new Troops(TroopType.Footmen, 20, 25));

            var attack = new Raid(attacker, defender, 10);

            attack.GetArmyStrengthModifier().Should().Be(1.2m);
        }

        [TestMethod]
        public void Attack_Defender_Always_Has_Minimum_Stamina() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            attacker.Troops.Add(new Troops(TroopType.Archers, 600, 200));
            defender.Troops.Add(new Troops(TroopType.Archers, 600, 200));

            typeof(Player).GetProperty(nameof(Player.Stamina)).SetValue(defender, 0);

            var attack = new GrandOverlordAttack(attacker, defender, 10);

            attack.Execute();

            attack.Rounds.Single(a => !a.IsAggressor).Damage.Should().Be(defender.Troops.Sum(t => t.GetTotals()) * 100);
        }
    }
}