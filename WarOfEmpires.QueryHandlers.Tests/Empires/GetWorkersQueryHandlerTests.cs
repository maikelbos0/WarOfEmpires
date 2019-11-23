using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
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

            _context.Users.Add(user);
            _context.Players.Add(player);
        }

        [TestMethod]
        public void GetWorkersQueryHandler_Returns_Correct_Peasants() {
            var command = new GetWorkersQuery("test@test.com");
            var handler = new GetWorkersQueryHandler(_context);

            var result = handler.Execute(command);

            result.CurrentPeasants.Should().Be(1);
        }

        [TestMethod]
        public void GetWorkersQueryHandler_Returns_Correct_Farmers() {
            var command = new GetWorkersQuery("test@test.com");
            var handler = new GetWorkersQueryHandler(_context);

            var result = handler.Execute(command);

            result.CurrentFarmers.Should().Be(2);
        }

        [TestMethod]
        public void GetWorkersQueryHandler_Returns_Correct_WoodWorkers() {
            var command = new GetWorkersQuery("test@test.com");
            var handler = new GetWorkersQueryHandler(_context);

            var result = handler.Execute(command);

            result.CurrentWoodWorkers.Should().Be(3);
        }

        [TestMethod]
        public void GetWorkersQueryHandler_Returns_Correct_StoneMasons() {
            var command = new GetWorkersQuery("test@test.com");
            var handler = new GetWorkersQueryHandler(_context);

            var result = handler.Execute(command);

            result.CurrentStoneMasons.Should().Be(4);
        }

        [TestMethod]
        public void GetWorkersQueryHandler_Returns_Correct_OreMiners() {
            var command = new GetWorkersQuery("test@test.com");
            var handler = new GetWorkersQueryHandler(_context);

            var result = handler.Execute(command);

            result.CurrentOreMiners.Should().Be(5);
        }
    }
}