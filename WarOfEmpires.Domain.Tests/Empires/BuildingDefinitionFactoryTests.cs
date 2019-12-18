using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Empires;

namespace WarOfEmpires.Domain.Tests.Empires {
    [TestClass]
    public class BuildingDefinitionFactoryTests {
        [TestMethod]
        public void BuildingDefinitionFactory_Has_Definitions_For_All_BuildingTypes() {
            foreach (BuildingType type in Enum.GetValues(typeof(BuildingType))) {
                var building = BuildingDefinitionFactory.Get(type);

                building.GetDescription(1);
                building.GetName(1);
                building.GetNextLevelCost(1);
            }
        }
    }
}