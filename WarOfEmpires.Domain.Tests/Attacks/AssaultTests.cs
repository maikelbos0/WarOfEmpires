using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Attacks {
    [TestClass]
    public sealed class AssaultTests {
        [TestMethod]
        public void Assault_Result_Is_Surrendered_For_Defender_Low_Stamina() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            attacker.TrainTroops(600, 200, 0, 0, 0, 0);
            defender.TrainTroops(600, 200, 0, 0, 0, 0);

            typeof(Player).GetProperty(nameof(Player.Stamina)).SetValue(defender, 29);

            var attack = new Assault(attacker, defender, 10);
            attack.Execute();

            attack.Result.Should().Be(AttackResult.Surrendered);
        }

        [TestMethod]
        public void Assault_Result_Is_Not_Surrendered_For_High_Defences_Low_Stamina() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void Assault_Surrendered_Gives_Correct_Resources_To_Attacker() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            attacker.TrainTroops(600, 200, 0, 0, 0, 0);
            defender.TrainTroops(600, 200, 0, 0, 0, 0);

            typeof(Player).GetProperty(nameof(Player.Stamina)).SetValue(defender, 29);

            var expectedResources = new Resources(defender.Resources.Gold) * 0.25m;
            var previousDefenderResources = defender.Resources;
            var previousAttackerResources = attacker.Resources;

            var attack = new Assault(attacker, defender, 10);
            attack.Execute();

            attack.Resources.Should().Be(expectedResources);
            defender.Resources.Should().Be(previousDefenderResources - expectedResources);
            attacker.Resources.Should().Be(previousAttackerResources + expectedResources);
        }

        [TestMethod]
        public void Assault_Won_Gives_Correct_Resources_To_Attacker() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            attacker.TrainTroops(600, 200, 0, 0, 0, 0);
            defender.TrainTroops(400, 100, 0, 0, 0, 0);

            var expectedResources = new Resources(defender.Resources.Gold) * 0.5m;
            var previousDefenderResources = defender.Resources;
            var previousAttackerResources = attacker.Resources;

            var attack = new Assault(attacker, defender, 10);
            attack.Execute();

            attack.Resources.Should().Be(expectedResources);
            defender.Resources.Should().Be(previousDefenderResources - expectedResources);
            attacker.Resources.Should().Be(previousAttackerResources + expectedResources);
        }
    }
}