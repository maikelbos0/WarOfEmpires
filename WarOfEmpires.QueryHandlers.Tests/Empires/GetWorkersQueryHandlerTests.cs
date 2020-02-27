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
                new Workers(WorkerType.Farmer, 2),
                new Workers(WorkerType.WoodWorker, 3),
                new Workers(WorkerType.StoneMason, 4),
                new Workers(WorkerType.OreMiner, 5),
                new Workers(WorkerType.SiegeEngineer, 6)
            });            
            player.Tax.Returns(50);
            player.GetRecruitsPerDay().Returns(5);
            player.GetUpkeepPerTurn().Returns(new Resources(gold: 500, food: 30));
            player.GetTotalResources().Returns(new Resources(gold: 1000, food: 100));
            player.GetResourcesPerTurn().Returns(new Resources(gold: 400, food: 20));
            player.GetBuildingBonus(BuildingType.SiegeFactory).Returns(4);
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

            result.CurrentFarmers.Should().Be(2);
            result.CurrentWoodWorkers.Should().Be(3);
            result.CurrentStoneMasons.Should().Be(4);
            result.CurrentOreMiners.Should().Be(5);
            result.CurrentSiegeEngineers.Should().Be(6);
        }

        [TestMethod]
        public void GetWorkersQueryHandler_Returns_Correct_Resources_Per_Turn() {
            var query = new GetWorkersQuery("test@test.com");
            var handler = new GetWorkersQueryHandler(_context, new ResourcesMap());

            var result = handler.Execute(query);

            result.CurrentGoldPerWorkerPerTurn.Should().Be(250);
            result.CurrentGoldPerTurn.Should().Be(3500);
            result.CurrentFoodPerWorkerPerTurn.Should().Be(10);
            result.CurrentFoodPerTurn.Should().Be(20);
            result.CurrentWoodPerWorkerPerTurn.Should().Be(10);
            result.CurrentWoodPerTurn.Should().Be(30);
            result.CurrentStonePerWorkerPerTurn.Should().Be(10);
            result.CurrentStonePerTurn.Should().Be(40);
            result.CurrentOrePerWorkerPerTurn.Should().Be(10);
            result.CurrentOrePerTurn.Should().Be(50);
            result.CurrentSiegeMaintenancePerSiegeEngineer.Should().Be(4);
            result.CurrentSiegeMaintenance.Should().Be(24);
        }

        [TestMethod]
        public void GetWorkersQueryHandler_Returns_Correct_Additional_Information() {
            var query = new GetWorkersQuery("test@test.com");
            var handler = new GetWorkersQueryHandler(_context, new ResourcesMap());

            var result = handler.Execute(query);

            result.RecruitsPerDay.Should().Be(5);
            result.UpkeepPerTurn.Food.Should().Be(30);
            result.UpkeepPerTurn.Gold.Should().Be(500);
            result.WorkerTrainingCost.Gold.Should().Be(250);
            result.SiegeEngineerTrainingCost.Gold.Should().Be(2500);
            result.SiegeEngineerTrainingCost.Wood.Should().Be(250);
            result.SiegeEngineerTrainingCost.Ore.Should().Be(500);
            result.WillUpkeepRunOut.Should().BeTrue();
            result.HasUpkeepRunOut.Should().BeTrue();
        }
    }
}