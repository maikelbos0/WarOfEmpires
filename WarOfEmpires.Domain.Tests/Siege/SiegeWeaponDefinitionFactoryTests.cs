using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Siege;

namespace WarOfEmpires.Domain.Tests.Siege {
    [TestClass]
    public sealed class SiegeWeaponDefinitionFactoryTests {
        [TestMethod]
        public void SiegeWeaponDefinitionFactory_Has_Definitions_For_All_SiegeWeaponTypes() {
            foreach (SiegeWeaponType type in Enum.GetValues(typeof(SiegeWeaponType))) {
                var weapon = SiegeWeaponDefinitionFactory.Get(type);

                weapon.Type.Should().Be(type);
            }
        }

        [TestMethod]
        public void SiegeWeaponDefinitionFactory_Has_Definitions_For_All_TroopTypes() {
            foreach (TroopType type in Enum.GetValues(typeof(TroopType))) {
                var weapon = SiegeWeaponDefinitionFactory.Get(type);

                weapon.TroopType.Should().Be(type);
            }
        }
    }
}