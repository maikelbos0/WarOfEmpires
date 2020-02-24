using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Attacks;

namespace WarOfEmpires.Domain.Tests.Attacks {
    [TestClass]
    public sealed class TroopDefinitionFactoryTests {
        [TestMethod]
        public void TroopDefinitionFactory_Has_Definitions_For_All_TroopTypes() {
            foreach (TroopType type in Enum.GetValues(typeof(TroopType))) {
                var weapon = TroopDefinitionFactory.Get(type);

                weapon.Type.Should().Be(type);
            }
        }
    }
}