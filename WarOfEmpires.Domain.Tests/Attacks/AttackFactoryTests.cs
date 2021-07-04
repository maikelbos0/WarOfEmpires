using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Attacks {
    [TestClass]
    public sealed class AttackFactoryTests {
        [TestMethod]
        public void AttackFactory_Resolves_All_AttackTypes() {
            foreach (AttackType type in Enum.GetValues(typeof(AttackType))) {
                var attack = AttackFactory.Get(type, new Player(1, "Attacker"), new Player(2, "Defender"), 10);

                attack.Should().NotBeNull();
            }
        }

        [TestMethod]
        public void AttackFactory_Returns_Raid_Correctly() {
            var attack = AttackFactory.Get(AttackType.Raid, new Player(1, "Attacker"), new Player(2, "Defender"), 10);

            attack.Type.Should().Be(AttackType.Raid);
            attack.Should().BeOfType<Raid>();
        }

        [TestMethod]
        public void AttackFactory_Returns_Assault_Correctly() {
            var attack = AttackFactory.Get(AttackType.Assault, new Player(1, "Attacker"), new Player(2, "Defender"), 10);

            attack.Type.Should().Be(AttackType.Assault);
            attack.Should().BeOfType<Assault>();
        }

        [TestMethod]
        public void AttackFactory_Returns_GrandOverlordAttack_Correctly() {
            var attack = AttackFactory.Get(AttackType.GrandOverlordAttack, new Player(1, "Attacker"), new Player(2, "Defender"), 10);

            attack.Type.Should().Be(AttackType.GrandOverlordAttack);
            attack.Should().BeOfType<GrandOverlordAttack>();
        }

        [TestMethod]
        public void AttackFactory_Returns_Revenge_Correctly() {
            var attack = AttackFactory.Get(AttackType.Revenge, new Player(1, "Attacker"), new Player(2, "Defender"), 10);

            attack.Type.Should().Be(AttackType.Revenge);
            attack.Should().BeOfType<Revenge>();
        }
    }
}