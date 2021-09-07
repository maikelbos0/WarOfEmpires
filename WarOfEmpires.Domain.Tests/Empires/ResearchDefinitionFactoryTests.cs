using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Empires;

namespace WarOfEmpires.Domain.Tests.Empires {
    [TestClass]
    public sealed class ResearchDefinitionFactoryTests {
        [TestMethod]
        public void ResearchDefinitionFactoryTests_Has_Definitions_For_All_ResearchTypes() {
            foreach (ResearchType type in Enum.GetValues(typeof(ResearchType))) {
                var research = ResearchDefinitionFactory.Get(type);

                research.Type.Should().Be(type);
            }
        }
    }
}