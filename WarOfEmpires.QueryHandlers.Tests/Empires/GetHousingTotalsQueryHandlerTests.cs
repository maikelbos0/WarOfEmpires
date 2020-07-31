using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Empires;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Empires {
    [TestClass]
    public sealed class GetHousingTotalsQueryHandlerTests {
        [TestMethod]
        public void GetHousingTotalsQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder().BuildPlayer(1)
                .WithPeasants(7)
                .WithWorkers(WorkerType.Farmers, 5)
                .WithWorkers(WorkerType.WoodWorkers, 12)
                .WithWorkers(WorkerType.StoneMasons, 2)
                .WithWorkers(WorkerType.OreMiners, 1)
                .WithWorkers(WorkerType.SiegeEngineers, 3)
                .WithTroops(TroopType.Archers, 25, 5)
                .WithTroops(TroopType.Cavalry, 5, 1)
                .WithTroops(TroopType.Footmen, 10, 2)
                .WithBuilding(BuildingType.Barracks, 7)
                .WithBuilding(BuildingType.Huts, 4);

            builder.Player.GetAvailableHousingCapacity().Returns(4);
            builder.Player.GetTheoreticalRecruitsPerDay().Returns(5);

            var handler = new GetHousingTotalsQueryHandler(builder.Context);
            var query = new GetHousingTotalsQuery("test1@test.com");

            var result = handler.Execute(query);

            result.BarracksCapacity.Should().Be(70);
            result.BarracksOccupancy.Should().Be(48);
            result.HutCapacity.Should().Be(40);
            result.HutOccupancy.Should().Be(23);
            result.TotalCapacity.Should().Be(110);
            result.TotalOccupancy.Should().Be(78);
            result.HasHousingShortage.Should().BeTrue();
        }
    }
}