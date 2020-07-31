using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Empires;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Empires {
    [TestClass]
    public sealed class GetBuildingTotalsQueryHandlerTests {        
        [TestMethod]
        public void GetBuildingTotalsQueryHandler_Returns_Correct_Values() {
            var builder = new FakeBuilder().BuildPlayer(1)
                .WithBuilding(BuildingType.Farm, 4)
                .WithBuilding(BuildingType.Lumberyard, 8)
                .WithBuilding(BuildingType.Quarry, 2)
                .WithBuilding(BuildingType.Mine, 2);

            var handler = new GetBuildingTotalsQueryHandler(builder.Context);
            var query = new GetBuildingTotalsQuery("test1@test.com");

            var result = handler.Execute(query);

            result.TotalGoldSpent.Should().Be(1580000);
            result.NextRecruitingLevel.Should().Be(2000000);
        }
    }
}