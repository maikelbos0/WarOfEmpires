using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class TrainWorkersCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Player _player;

        public TrainWorkersCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);

            var player = Substitute.For<Player>();
            player.User.Returns(user);
            player.Peasants.Returns(10);
            player.CanAfford(Arg.Any<Resources>()).Returns(true);
            player.GetAvailableHutCapacity().Returns(20);

            _context.Users.Add(user);
            _context.Players.Add(player);
            _player = player;
        }

        [TestMethod]
        public void TrainWorkersCommandHandler_Succeeds() {
            var handler = new TrainWorkersCommandHandler(_repository);
            var command = new TrainWorkersCommand("test@test.com", "0", "1", "2", "3");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.Received().TrainWorkers(0, 1, 2, 3);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void TrainWorkersCommandHandler_Allows_Empty_Workers() {
            var handler = new TrainWorkersCommandHandler(_repository);
            var command = new TrainWorkersCommand("test@test.com", "", "", "", "");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.Received().TrainWorkers(0, 0, 0, 0);
        }

        [TestMethod]
        public void TrainWorkersCommandHandler_Fails_For_Too_Little_Huts_Room() {
            _player.Peasants.Returns(30);

            var handler = new TrainWorkersCommandHandler(_repository);
            var command = new TrainWorkersCommand("test@test.com", "7", "7", "7", "7");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.Should().BeNull();
            result.Errors[0].Message.Should().Be("You don't have enough huts available to train that many workers");
            _player.DidNotReceiveWithAnyArgs().TrainWorkers(default, default, default, default);
        }

        [TestMethod]
        public void TrainWorkersCommandHandler_Fails_For_Alphanumeric_Farmers() {
            var handler = new TrainWorkersCommandHandler(_repository);
            var command = new TrainWorkersCommand("test@test.com", "A", "2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Farmers");
            result.Errors[0].Message.Should().Be("Farmers must be a valid number");
            _player.DidNotReceiveWithAnyArgs().TrainWorkers(default, default, default, default);
        }

        [TestMethod]
        public void TrainWorkersCommandHandler_Fails_For_Alphanumeric_WoodWorkers() {
            var handler = new TrainWorkersCommandHandler(_repository);
            var command = new TrainWorkersCommand("test@test.com", "2", "A", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.WoodWorkers");
            result.Errors[0].Message.Should().Be("Wood workers must be a valid number");
            _player.DidNotReceiveWithAnyArgs().TrainWorkers(default, default, default, default);
        }

        [TestMethod]
        public void TrainWorkersCommandHandler_Fails_For_Alphanumeric_StoneMasons() {
            var handler = new TrainWorkersCommandHandler(_repository);
            var command = new TrainWorkersCommand("test@test.com", "2", "2", "A", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.StoneMasons");
            result.Errors[0].Message.Should().Be("Stone masons must be a valid number");
            _player.DidNotReceiveWithAnyArgs().TrainWorkers(default, default, default, default);
        }

        [TestMethod]
        public void TrainWorkersCommandHandler_Fails_For_Alphanumeric_OreMiners() {
            var handler = new TrainWorkersCommandHandler(_repository);
            var command = new TrainWorkersCommand("test@test.com", "2", "2", "2", "A");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.OreMiners");
            result.Errors[0].Message.Should().Be("Ore miners must be a valid number");
            _player.DidNotReceiveWithAnyArgs().TrainWorkers(default, default, default, default);
        }

        [DataTestMethod]
        [DataRow(11, 0, 0, 0, DisplayName = "Farmers")]
        [DataRow(0, 11, 0, 0, DisplayName = "WoodWorkers")]
        [DataRow(0, 0, 11, 0, DisplayName = "StoneMasons")]
        [DataRow(0, 0, 0, 11, DisplayName = "OreMiners")]
        [DataRow(3, 3, 3, 3, DisplayName = "All")]
        public void TrainWorkersCommandHandler_Fails_For_Too_High_WorkerCounts(int farmers, int woodWorkers, int stoneMasons, int oreMiners) {
            var handler = new TrainWorkersCommandHandler(_repository);
            var command = new TrainWorkersCommand("test@test.com", farmers.ToString(), woodWorkers.ToString(), stoneMasons.ToString(), oreMiners.ToString());

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.Should().BeNull();
            result.Errors[0].Message.Should().Be("You don't have that many peasants available to train");
            _player.DidNotReceiveWithAnyArgs().TrainWorkers(default, default, default, default);
        }

        [TestMethod]
        public void TrainWorkersCommandHandler_Fails_For_Negative_Farmers() {
            var handler = new TrainWorkersCommandHandler(_repository);
            var command = new TrainWorkersCommand("test@test.com", "-7", "2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Farmers");
            result.Errors[0].Message.Should().Be("Farmers must be a valid number");
            _player.DidNotReceiveWithAnyArgs().TrainWorkers(default, default, default, default);
        }

        [TestMethod]
        public void TrainWorkersCommandHandler_Fails_For_Negative_WoodWorkers() {
            var handler = new TrainWorkersCommandHandler(_repository);
            var command = new TrainWorkersCommand("test@test.com", "2", "-7", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.WoodWorkers");
            result.Errors[0].Message.Should().Be("Wood workers must be a valid number");
            _player.DidNotReceiveWithAnyArgs().TrainWorkers(default, default, default, default);
        }

        [TestMethod]
        public void TrainWorkersCommandHandler_Fails_For_Negative_StoneMasons() {
            var handler = new TrainWorkersCommandHandler(_repository);
            var command = new TrainWorkersCommand("test@test.com", "2", "2", "-7", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.StoneMasons");
            result.Errors[0].Message.Should().Be("Stone masons must be a valid number");
            _player.DidNotReceiveWithAnyArgs().TrainWorkers(default, default, default, default);
        }

        [TestMethod]
        public void TrainWorkersCommandHandler_Fails_For_Negative_OreMiners() {
            var handler = new TrainWorkersCommandHandler(_repository);
            var command = new TrainWorkersCommand("test@test.com", "2", "2", "2", "-7");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.OreMiners");
            result.Errors[0].Message.Should().Be("Ore miners must be a valid number");
            _player.DidNotReceiveWithAnyArgs().TrainWorkers(default, default, default, default);
        }

        [TestMethod]
        public void TrainWorkersCommandHandler_Fails_For_Too_Little_Resources() {
            _player.CanAfford(Arg.Any<Resources>()).Returns(false);

            var handler = new TrainWorkersCommandHandler(_repository);
            var command = new TrainWorkersCommand("test@test.com", "2", "2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.Should().BeNull();
            result.Errors[0].Message.Should().Be("You don't have enough gold to train these peasants");
            _player.DidNotReceiveWithAnyArgs().TrainWorkers(default, default, default, default);
        }
    }
}