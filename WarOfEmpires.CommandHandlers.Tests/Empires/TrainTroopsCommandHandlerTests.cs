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
    public sealed class TrainTroopsCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Player _player;

        public TrainTroopsCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);

            var player = Substitute.For<Player>();
            player.GetBarracksCapacity().Returns(20);
            player.User.Returns(user);
            player.Peasants.Returns(30);
            player.Resources.Returns(new Resources(gold: 500000, wood: 50000, ore: 100000));

            _context.Users.Add(user);
            _context.Players.Add(player);
            _player = player;
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Succeeds() {
            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", "5", "3", "4", "2", "4", "1");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.Received().TrainTroops(5, 3, 4, 2, 4, 1);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Allows_Empty_Troops() {
            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", "", "", "", "", "", "");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.Received().TrainTroops(0, 0, 0, 0, 0, 0);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Too_Little_Barracks_Room() {
            _player.Peasants.Returns(30);

            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", "8", "2", "8", "2", "8", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.Should().BeNull();
            result.Errors[0].Message.Should().Be("You don't have enough barracks available to train that many troops");
            _player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default, default, default, default);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Alphanumeric_Archers() {
            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", "A", "2", "2", "2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Archers");
            result.Errors[0].Message.Should().Be("Archers must be a valid number");
            _player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default, default, default, default);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Alphanumeric_MercenaryArchers() {
            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", "2", "A", "2", "2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.MercenaryArchers");
            result.Errors[0].Message.Should().Be("Archer mercenaries must be a valid number");
            _player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default, default, default, default);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Alphanumeric_Cavalry() {
            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", "2", "2", "A", "2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Cavalry");
            result.Errors[0].Message.Should().Be("Cavalry must be a valid number");
            _player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default, default, default, default);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Alphanumeric_MercenaryCavalry() {
            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", "2", "2", "2", "A", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.MercenaryCavalry");
            result.Errors[0].Message.Should().Be("Cavalry mercenaries must be a valid number");
            _player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default, default, default, default);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Alphanumeric_Footmen() {
            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", "2", "2", "2", "2", "A", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Footmen");
            result.Errors[0].Message.Should().Be("Footmen must be a valid number");
            _player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default, default, default, default);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Alphanumeric_MercenaryFootmen() {
            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", "2", "2", "2", "2", "2", "A");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.MercenaryFootmen");
            result.Errors[0].Message.Should().Be("Footman mercenaries must be a valid number");
            _player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default, default, default, default);
        }

        [DataTestMethod]
        [DataRow(31, 0, 0, 0, 0, 0, DisplayName = "Archers")]
        [DataRow(0, 0, 31, 0, 0, 0, DisplayName = "Cavalry")]
        [DataRow(0, 0, 0, 0, 31, 0, DisplayName = "Footmen")]
        [DataRow(11, 0, 11, 0, 11, 0, DisplayName = "All")]
        public void TrainTroopsCommandHandler_Fails_For_Too_High_TroopCounts(int archers, int mercenaryArchers, int cavalry, int mercenaryCavalry, int footmen, int mercenaryFootmen) {
            _player.GetBarracksCapacity().Returns(40);

            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", archers.ToString(), mercenaryArchers.ToString(), cavalry.ToString(), mercenaryCavalry.ToString(), footmen.ToString(), mercenaryFootmen.ToString());

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.Should().BeNull();
            result.Errors[0].Message.Should().Be("You don't have that many peasants available to train");
            _player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default, default, default, default);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Negative_Archers() {
            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", "-2", "2", "2", "2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Archers");
            result.Errors[0].Message.Should().Be("Archers must be a valid number");
            _player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default, default, default, default);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Negative_MercenaryArchers() {
            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", "2", "-2", "2", "2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.MercenaryArchers");
            result.Errors[0].Message.Should().Be("Archer mercenaries must be a valid number");
            _player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default, default, default, default);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Negative_Cavalry() {
            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", "2", "2", "-2", "2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Cavalry");
            result.Errors[0].Message.Should().Be("Cavalry must be a valid number");
            _player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default, default, default, default);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Negative_MercenaryCavalry() {
            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", "2", "2", "2", "-2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.MercenaryCavalry");
            result.Errors[0].Message.Should().Be("Cavalry mercenaries must be a valid number");
            _player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default, default, default, default);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Negative_Footmen() {
            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", "2", "2", "2", "2", "-2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Footmen");
            result.Errors[0].Message.Should().Be("Footmen must be a valid number");
            _player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default, default, default, default);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Negative_MercenaryFootmen() {
            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", "2", "2", "2", "2", "2", "-2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.MercenaryFootmen");
            result.Errors[0].Message.Should().Be("Footman mercenaries must be a valid number");
            _player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default, default, default, default);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Too_Little_Gold() {
            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", "2", "2", "2", "2", "2", "2");

            _player.Resources.Returns(new Resources(gold: 1000, wood: 3000, ore: 6000));

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.Should().BeNull();
            result.Errors[0].Message.Should().Be("You don't have enough resources to train these troops");
            _player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default, default, default, default);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Too_Little_Wood() {
            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", "2", "2", "2", "2", "2", "2");

            _player.Resources.Returns(new Resources(gold: 60000, wood: 1000, ore: 6000));

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.Should().BeNull();
            result.Errors[0].Message.Should().Be("You don't have enough resources to train these troops");
            _player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default, default, default, default);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Too_Little_Ore() {
            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", "2", "2", "2", "2", "2", "2");

            _player.Resources.Returns(new Resources(gold: 60000, wood: 3000, ore: 1000));

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.Should().BeNull();
            result.Errors[0].Message.Should().Be("You don't have enough resources to train these troops");
            _player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default, default, default, default);
        }
    }
}
