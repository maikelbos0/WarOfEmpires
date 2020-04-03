using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.QueryHandlers.Empires;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Tests.Empires {
    [TestClass]
    public sealed class GetWorkersQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly ResourcesMap _resourcesMap = new ResourcesMap();
        private readonly EnumFormatter _formatter = new EnumFormatter();

        public GetWorkersQueryHandlerTests() {
            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();

            user.Id.Returns(1);
            user.Status.Returns(UserStatus.Active);
            user.Email.Returns("test@test.com");

            player.User.Returns(user);
            player.Id.Returns(1);
            player.Peasants.Returns(1);
            player.Workers.Returns(new List<Workers>() {
                new Workers(WorkerType.Farmers, 2),
                new Workers(WorkerType.WoodWorkers, 3),
                new Workers(WorkerType.StoneMasons, 4),
                new Workers(WorkerType.OreMiners, 5),
                new Workers(WorkerType.SiegeEngineers, 6),
                new Workers(WorkerType.Merchants, 7),
            });            
            player.Tax.Returns(50);
            player.GetRecruitsPerDay().Returns(5);
            player.GetUpkeepPerTurn().Returns(new Resources(gold: 500, food: 30));
            player.GetTotalResources().Returns(new Resources(gold: 1000, food: 100));
            player.GetResourcesPerTurn().Returns(new Resources(gold: 400, food: 20));
            player.GetBuildingBonus(BuildingType.SiegeFactory).Returns(4);
            player.GetBuildingBonus(BuildingType.Market).Returns(10000);
            player.HasUpkeepRunOut.Returns(true);

            _context.Users.Add(user);
            _context.Players.Add(player);
        }

        [TestMethod]
        public void GetWorkersQueryHandler_Returns_Correct_Peasants() {
            var query = new GetWorkersQuery("test@test.com");
            var handler = new GetWorkersQueryHandler(_context, _resourcesMap, _formatter);

            var result = handler.Execute(query);

            result.CurrentPeasants.Should().Be(1);
        }

        [TestMethod]
        public void GetWorkersQueryHandler_Returns_Correct_Farmers() {
            var query = new GetWorkersQuery("test@test.com");
            var handler = new GetWorkersQueryHandler(_context, _resourcesMap, _formatter);

            var result = handler.Execute(query);
            var worker = result.Workers.Single(s => s.Type == "Farmers");

            worker.CurrentWorkers.Should().Be(2);
            worker.CurrentProductionPerWorkerPerTurn.Should().Be(10);
            worker.CurrentProductionPerTurn.Should().Be(20);
        }

        [TestMethod]
        public void GetWorkersQueryHandler_Returns_Correct_WoodWorkers() {
            var query = new GetWorkersQuery("test@test.com");
            var handler = new GetWorkersQueryHandler(_context, _resourcesMap, _formatter);

            var result = handler.Execute(query);
            var worker = result.Workers.Single(s => s.Type == "WoodWorkers");

            worker.CurrentWorkers.Should().Be(3);
            worker.CurrentProductionPerWorkerPerTurn.Should().Be(10);
            worker.CurrentProductionPerTurn.Should().Be(30);
        }

        [TestMethod]
        public void GetWorkersQueryHandler_Returns_Correct_Stonemasons() {
            var query = new GetWorkersQuery("test@test.com");
            var handler = new GetWorkersQueryHandler(_context, _resourcesMap, _formatter);

            var result = handler.Execute(query);
            var worker = result.Workers.Single(s => s.Type == "StoneMasons");

            worker.CurrentWorkers.Should().Be(4);
            worker.CurrentProductionPerWorkerPerTurn.Should().Be(10);
            worker.CurrentProductionPerTurn.Should().Be(40);
        }

        [TestMethod]
        public void GetWorkersQueryHandler_Returns_Correct_OreMiners() {
            var query = new GetWorkersQuery("test@test.com");
            var handler = new GetWorkersQueryHandler(_context, _resourcesMap, _formatter);

            var result = handler.Execute(query);
            var worker = result.Workers.Single(s => s.Type == "OreMiners");

            worker.CurrentWorkers.Should().Be(5);
            worker.CurrentProductionPerWorkerPerTurn.Should().Be(10);
            worker.CurrentProductionPerTurn.Should().Be(50);
        }

        [TestMethod]
        public void GetWorkersQueryHandler_Returns_Correct_SiegeEngineers() {
            var query = new GetWorkersQuery("test@test.com");
            var handler = new GetWorkersQueryHandler(_context, _resourcesMap, _formatter);

            var result = handler.Execute(query);
            var worker = result.Workers.Single(s => s.Type == "SiegeEngineers");

            worker.CurrentWorkers.Should().Be(6);
            worker.CurrentProductionPerWorkerPerTurn.Should().Be(4);
            worker.CurrentProductionPerTurn.Should().Be(24);
            worker.Cost.Gold.Should().Be(2500);
            worker.Cost.Wood.Should().Be(250);
            worker.Cost.Ore.Should().Be(500);
        }

        [TestMethod]
        public void GetWorkersQueryHandler_Returns_Correct_Merchants() {
            var query = new GetWorkersQuery("test@test.com");
            var handler = new GetWorkersQueryHandler(_context, _resourcesMap, _formatter);

            var result = handler.Execute(query);
            var worker = result.Workers.Single(s => s.Type == "Merchants");

            worker.CurrentWorkers.Should().Be(7);
            worker.CurrentProductionPerWorkerPerTurn.Should().Be(10000);
            worker.CurrentProductionPerTurn.Should().Be(70000);
            worker.Cost.Gold.Should().Be(2500);
            worker.Cost.Wood.Should().Be(500);
            worker.Cost.Ore.Should().Be(250);
        }

        [TestMethod]
        public void GetWorkersQueryHandler_Returns_Correct_Additional_Information() {
            var query = new GetWorkersQuery("test@test.com");
            var handler = new GetWorkersQueryHandler(_context, _resourcesMap, _formatter);

            var result = handler.Execute(query);

            result.CurrentGoldPerWorkerPerTurn.Should().Be(250);
            result.CurrentGoldPerTurn.Should().Be(3500);
            result.RecruitsPerDay.Should().Be(5);
            result.UpkeepPerTurn.Food.Should().Be(30);
            result.UpkeepPerTurn.Gold.Should().Be(500);
            result.WorkerCost.Gold.Should().Be(250);
            result.WillUpkeepRunOut.Should().BeTrue();
            result.HasUpkeepRunOut.Should().BeTrue();
        }
    }
}