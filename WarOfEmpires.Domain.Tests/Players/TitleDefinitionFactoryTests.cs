using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Players {
    [TestClass]
    public sealed class TitleDefinitionFactoryTests {
        [TestMethod]
        public void TitleDefinitionFactory_Has_Definitions_For_All_TitleTypes() {
            foreach (TitleType type in Enum.GetValues(typeof(TitleType))) {
                var title = TitleDefinitionFactory.Get(type);

                title.Type.Should().Be(type);
            }
        }

        [TestMethod]
        public void TitleDefinitionFactory_GetAll_Succeeds() {
            TitleDefinitionFactory.GetAll().Should().HaveCount(Enum.GetValues(typeof(TitleType)).Length);
        }
    }
}