using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.QueryHandlers.Empires;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Empires {
    [TestClass]
    public sealed class GetBuildingQueryHandlerTests {
        [TestMethod]
        public void GetBuildingQueryHandler_Returns_Correct_Values_For_Existing_Building() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithBuilding(BuildingType.Lumberyard, 2);

            var handler = new GetBuildingQueryHandler(builder.Context, new ResourcesMap());
            var query = new GetBuildingQuery("test1@test.com", "Lumberyard");

            var result = handler.Execute(query);

            result.Level.Should().Be(2);
            result.Name.Should().Be("Lumberyard (level 2)");
            result.Description.Should().Be("Your lumberyard increases wood production by 25% for each level; your current bonus is 50%");
            result.UpdateCost.Gold.Should().Be(50000);
        }

        [TestMethod]
        public void GetBuildingQueryHandler_Returns_Correct_Values_For_New_Building() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithBuilding(BuildingType.Lumberyard, 2);

            var handler = new GetBuildingQueryHandler(builder.Context, new ResourcesMap());
            var query = new GetBuildingQuery("test1@test.com", "Farm");

            var result = handler.Execute(query);

            result.Level.Should().Be(0);
            result.Name.Should().Be("Farm (level 0)");
            result.Description.Should().Be("Your farm increases food production by 25% for each level; your current bonus is 0%");
            result.UpdateCost.Gold.Should().Be(20000);
        }

        [TestMethod]
        public void GetBuildingQueryHandler_Throws_Exception_For_Invalid_BuildingType() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new GetBuildingQueryHandler(builder.Context, new ResourcesMap());
            var query = new GetBuildingQuery("test1@test.com", "Wrong");

            Action action = () => handler.Execute(query);

            action.Should().Throw<ArgumentException>();
        }
    }
}