using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Common;
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

        [TestMethod]
        public void BuildingDefinitionFactory_Get_Resource_Totals() {
            var level1Resources = new Resources();
            var level15Resources = new Resources();

            foreach (BuildingType type in Enum.GetValues(typeof(BuildingType))) {
                var building = BuildingDefinitionFactory.Get(type);

                level1Resources +=building.GetNextLevelCost(1);
                level15Resources += building.GetNextLevelCost(15);
            }

            Console.WriteLine($"Gold: {level1Resources.Gold:N0} - {level15Resources.Gold:N0}");
            Console.WriteLine($"Food: {level1Resources.Food:N0} - {level15Resources.Food:N0}");
            Console.WriteLine($"Wood: {level1Resources.Wood:N0} - {level15Resources.Wood:N0}");
            Console.WriteLine($"Stone: {level1Resources.Stone:N0} - {level15Resources.Stone:N0}");
            Console.WriteLine($"Ore: {level1Resources.Ore:N0} - {level15Resources.Ore:N0}");
        }
    }
}