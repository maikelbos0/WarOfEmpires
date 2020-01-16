using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Siege;

namespace WarOfEmpires.Domain.Tests.Siege {
    [TestClass]
    public sealed class SiegeWeaponDefinitionFactoryTests {
        [TestMethod]
        public void SiegeWeaponDefinitionFactory_Has_Definitions_For_All_SiegeWeaponTypes() {
            foreach (SiegeWeaponType type in Enum.GetValues(typeof(SiegeWeaponType))) {
                var weapon = SiegeWeaponDefinitionFactory.Get(type);
            }
        }
    }
}