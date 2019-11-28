using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Empires;
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
            player.Farmers.Returns(2);
            player.WoodWorkers.Returns(3);
            player.StoneMasons.Returns(4);
            player.OreMiners.Returns(5);
            player.Tax.Returns(50);
            player.GetRecruitsPerDay().Returns(5);
            player.GetFoodCostPerTurn().Returns(new Resources(food: 30));

            _context.Users.Add(user);
            _context.Players.Add(player);
        }

        [TestMethod]
        public void GetWorkersQueryHandler_Returns_Correct_Peasants() {
            var query = new GetWorkersQuery("test@test.com");
            var handler = new GetWorkersQueryHandler(_context);

            var result = handler.Execute(query);

            result.CurrentPeasants.Should().Be(1);
        }

        [TestMethod]
        public void GetWorkersQueryHandler_Returns_Correct_Workers() {
            var query = new GetWorkersQuery("test@test.com");
            var handler = new GetWorkersQueryHandler(_context);

            var result = handler.Execute(query);

            result.CurrentFarmers.Should().Be(2);
            result.CurrentWoodWorkers.Should().Be(3);
            result.CurrentStoneMasons.Should().Be(4);
            result.CurrentOreMiners.Should().Be(5);
        }

        [TestMethod]
        public void GetWorkersQueryHandler_Returns_Correct_Resources_Per_Turn() {
            var query = new GetWorkersQuery("test@test.com");
            var handler = new GetWorkersQueryHandler(_context);

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
        }

        [TestMethod]
        public void GetWorkersQueryHandler_Returns_Correct_Additional_Information() {
            var query = new GetWorkersQuery("test@test.com");
            var handler = new GetWorkersQueryHandler(_context);

            var result = handler.Execute(query);

            result.RecruitsPerDay.Should().Be(5);
            result.FoodCostPerTurn.Should().Be(30);
        }
    }
}