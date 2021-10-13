using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Players {
    [TestClass]
    public sealed class RaceDefinitionFactoryTests {
        [TestMethod]
        public void RaceDefinitionFactory_Has_Definitions_For_All_Races() {
            foreach (Race race in Enum.GetValues(typeof(Race))) {
                var definition = RaceDefinitionFactory.Get(race);

                definition.Race.Should().Be(race);
            }
        }

        [TestMethod]
        public void RaceDefinitionFactory_GetAll_Succeeds() {
            RaceDefinitionFactory.GetAll().Should().HaveSameCount(Enum.GetValues(typeof(Race)));
        }
    }
}