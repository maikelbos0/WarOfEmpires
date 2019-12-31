using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Attacks;

namespace WarOfEmpires.Domain.Tests.Attacks {
    [TestClass]
    public sealed class AttackFactoryTests {
        [TestMethod]
        public void AttackFactory_Resolves_All_AttackTypes() {
            foreach (AttackType type in Enum.GetValues(typeof(AttackType))) {
                var attack = AttackFactory.Get(type, null, null, 10);

                attack.Should().NotBeNull();
            }
        }

        [TestMethod]
        public void AttackFactory_Returns_Raid_Correctly() {
            var attack = AttackFactory.Get(AttackType.Raid, null, null, 10);

            attack.Type.Should().Be(AttackType.Raid);
            attack.Should().BeOfType<Raid>();
        }

        [TestMethod]
        public void AttackFactory_Returns_CastleAttack_Correctly() {
            var attack = AttackFactory.Get(AttackType.CastleAttack, null, null, 10);

            attack.Type.Should().Be(AttackType.CastleAttack);
            attack.Should().BeOfType<CastleAttack>();
        }
    }
}