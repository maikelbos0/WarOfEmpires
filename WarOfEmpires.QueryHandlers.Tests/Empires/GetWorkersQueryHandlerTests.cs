using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Linq;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.QueryHandlers.Empires;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Tests.Empires {
    [TestClass]
    public sealed class GetWorkersQueryHandlerTests {

        [TestMethod]
        public void GetWorkersQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithPeasants(1)
                .WithWorkers(WorkerType.Farmers, 2)
                .WithWorkers(WorkerType.WoodWorkers, 3)
                .WithWorkers(WorkerType.StoneMasons, 4)
                .WithWorkers(WorkerType.OreMiners, 5)
                .WithWorkers(WorkerType.SiegeEngineers, 6)
                .WithWorkers(WorkerType.Merchants, 7)
                .WithBuilding(BuildingType.SiegeFactory, 4)
                .WithBuilding(BuildingType.Market, 1);

            builder.Player.Tax.Returns(50);
            builder.Player.GetRecruitsPerDay().Returns(5);
            builder.Player.WillUpkeepRunOut().Returns(true);
            builder.Player.GetUpkeepPerTurn().Returns(new Resources(gold: 500, food: 30));
            builder.Player.GetResourcesPerTurn().Returns(new Resources(gold: 400, food: 20));
            builder.Player.HasUpkeepRunOut.Returns(true);

            var handler = new GetWorkersQueryHandler(builder.Context, new ResourcesMap(), new EnumFormatter());
            var query = new GetWorkersQuery("test1@test.com");

            var result = handler.Execute(query);

            result.CurrentPeasants.Should().Be(1);
            result.CurrentGoldPerWorkerPerTurn.Should().Be(250);
            result.CurrentGoldPerTurn.Should().Be(3500);
            result.RecruitsPerDay.Should().Be(5);
            result.UpkeepPerTurn.Food.Should().Be(30);
            result.UpkeepPerTurn.Gold.Should().Be(500);
            result.WorkerCost.Gold.Should().Be(250);
            result.WillUpkeepRunOut.Should().BeTrue();
            result.Workers.Should().HaveCount(7);

            var woodWorkers = result.Workers.Single(s => s.Type == "WoodWorkers");

            woodWorkers.Name.Should().Be("Wood workers");
            woodWorkers.CurrentWorkers.Should().Be(3);
            woodWorkers.CurrentProductionPerWorkerPerTurn.Should().Be(13);
            woodWorkers.CurrentProductionPerTurn.Should().Be(39);

            var merchants = result.Workers.Single(s => s.Type == "Merchants");

            merchants.Name.Should().Be("Merchants");
            merchants.CurrentWorkers.Should().Be(7);
            merchants.CurrentProductionPerWorkerPerTurn.Should().Be(10000);
            merchants.CurrentProductionPerTurn.Should().Be(70000);
            merchants.Cost.Gold.Should().Be(2500);
            merchants.Cost.Wood.Should().Be(500);
            merchants.Cost.Ore.Should().Be(250);
        }
    }
}