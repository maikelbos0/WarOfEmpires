using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Attacks {
    [TestClass]
    public sealed class AttackTests {
        [TestMethod]
        public void MyTestMethod() {
            var attacker = new Player(1, "Attacker");
            var defender = new Player(2, "Defender");

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            typeof(Player).GetProperty(nameof(Player.Peasants)).SetValue(attacker, 1000);
            typeof(Player).GetProperty(nameof(Player.Peasants)).SetValue(defender, 1000);

            attacker.TrainTroops(600, 200, 40, 5, 40, 5);
            defender.TrainTroops(600, 200, 40, 5, 40, 5);

            var attack = new Attack(attacker, defender, 10);
            attack.Execute();


        }
    }
}