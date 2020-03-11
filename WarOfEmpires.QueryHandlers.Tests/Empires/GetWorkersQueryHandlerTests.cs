using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.QueryHandlers.Empires;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Empires {
    [TestClass]
    public sealed class GetWorkersQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();

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
            var handler = new GetWorkersQueryHandler(_context, new ResourcesMap());

            var result = handler.Execute(query);

            result.CurrentPeasants.Should().Be(1);
        }

        [TestMethod]
        public void GetWorkersQueryHandler_Returns_Correct_Workers() {
            var query = new GetWorkersQuery("test@test.com");
            var handler = new GetWorkersQueryHandler(_context, new ResourcesMap());

            var result = handler.Execute(query);

            result.FarmerInfo.CurrentWorkers.Should().Be(2);
            result.WoodWorkerInfo.CurrentWorkers.Should().Be(3);
            result.StoneMasonInfo.CurrentWorkers.Should().Be(4);
            result.OreMinerInfo.CurrentWorkers.Should().Be(5);
            result.SiegeEngineerInfo.CurrentWorkers.Should().Be(6);
            result.MerchantInfo.CurrentWorkers.Should().Be(7);
        }

        [TestMethod]
        public void GetWorkersQueryHandler_Returns_Correct_Resources_Per_Turn() {
            var query = new GetWorkersQuery("test@test.com");
            var handler = new GetWorkersQueryHandler(_context, new ResourcesMap());

            var result = handler.Execute(query);

            result.CurrentGoldPerWorkerPerTurn.Should().Be(250);
            result.CurrentGoldPerTurn.Should().Be(3500);
            result.FarmerInfo.CurrentProductionPerWorkerPerTurn.Should().Be(10);
            result.FarmerInfo.CurrentProductionPerTurn.Should().Be(20);
            result.WoodWorkerInfo.CurrentProductionPerWorkerPerTurn.Should().Be(10);
            result.WoodWorkerInfo.CurrentProductionPerTurn.Should().Be(30);
            result.StoneMasonInfo.CurrentProductionPerWorkerPerTurn.Should().Be(10);
            result.StoneMasonInfo.CurrentProductionPerTurn.Should().Be(40);
            result.OreMinerInfo.CurrentProductionPerWorkerPerTurn.Should().Be(10);
            result.OreMinerInfo.CurrentProductionPerTurn.Should().Be(50);
            result.SiegeEngineerInfo.CurrentProductionPerWorkerPerTurn.Should().Be(4);
            result.SiegeEngineerInfo.CurrentProductionPerTurn.Should().Be(24);
            result.MerchantInfo.CurrentProductionPerWorkerPerTurn.Should().Be(10000);
            result.MerchantInfo.CurrentProductionPerTurn.Should().Be(70000);
        }

        [TestMethod]
        public void GetWorkersQueryHandler_Returns_Correct_Additional_Information() {
            var query = new GetWorkersQuery("test@test.com");
            var handler = new GetWorkersQueryHandler(_context, new ResourcesMap());

            var result = handler.Execute(query);

            result.RecruitsPerDay.Should().Be(5);
            result.UpkeepPerTurn.Food.Should().Be(30);
            result.UpkeepPerTurn.Gold.Should().Be(500);
            result.FarmerInfo.Cost.Gold.Should().Be(250);
            result.SiegeEngineerInfo.Cost.Gold.Should().Be(2500);
            result.SiegeEngineerInfo.Cost.Wood.Should().Be(250);
            result.SiegeEngineerInfo.Cost.Ore.Should().Be(500);
            result.MerchantInfo.Cost.Gold.Should().Be(2500);
            result.MerchantInfo.Cost.Wood.Should().Be(500);
            result.MerchantInfo.Cost.Ore.Should().Be(250);
            result.WillUpkeepRunOut.Should().BeTrue();
            result.HasUpkeepRunOut.Should().BeTrue();
        }
    }
}